using System.Text;
using System.Text.Json;

namespace LegacyProcs.Services.AI;

/// <summary>
/// Implementação do serviço Gemini AI
/// ✨ NOVA FEATURE - Integração com Google Gemini para assistência inteligente
/// </summary>
public class GeminiService : IGeminiService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GeminiService> _logger;
    private readonly HttpClient _httpClient;

    public GeminiService(
        IConfiguration configuration,
        ILogger<GeminiService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("Gemini");
    }

    public async Task<string> GerarDescricaoAsync(string titulo)
    {
        var prompt = $@"Descreva em 3-4 passos COMO fazer:

{titulo}

Formato:
PASSO 1: [ação]
PASSO 2: [ação]
PASSO 3: [ação]

Materiais: [lista curta]

Máximo 80 palavras. Seja direto.";

        return await CallGeminiAsync(prompt);
    }

    public async Task<string> SugerirTecnicoAsync(string descricao, List<string> tecnicosDisponiveis)
    {
        var listaTecnicos = string.Join("\n", tecnicosDisponiveis.Select((t, i) => $"{i + 1}. {t}"));
        
        var prompt = $@"Escolha o técnico adequado:

Serviço: {descricao}

Técnicos:
{listaTecnicos}

Regras:
- Ar Condicionado: ar, climatização
- Hidráulica: água, vazamento, cano
- Elétrica: luz, tomada, fiação
- Informática: computador, rede, TI
- Geral: limpeza, pintura, reparo

Responda APENAS o NOME do técnico ou NENHUM se não houver match.
Exemplo: João Silva";

        return await CallGeminiAsync(prompt);
    }

    public async Task<string> AnalisarPrioridadeAsync(string descricao)
    {
        var prompt = $@"Classifique a urgência:

{descricao}

Categorias:
- URGENTE: crítico, perigo, parado
- ALTA: importante, precisa atenção
- MÉDIA: pode aguardar dias
- BAIXA: não urgente

Responda APENAS: URGENTE, ALTA, MÉDIA ou BAIXA";

        return await CallGeminiAsync(prompt);
    }

    public async Task<string> EstimarTempoAsync(string descricao)
    {
        var prompt = $@"Estime o tempo:

{descricao}

Opções:
- Rápido: 30min, 1h, 2h
- Médio: 4h, 1 dia
- Longo: 2-3 dias, 1 semana

Responda formato: 2h ou 1 dia";

        return await CallGeminiAsync(prompt);
    }

    private async Task<string> CallGeminiAsync(string prompt)
    {
        try
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("Gemini API Key não configurada");
                return "IA não disponível - Configure a API Key";
            }

            var model = _configuration["Gemini:Model"] ?? "gemini-1.5-pro";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 1024
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Chamando Gemini API...");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Erro na API Gemini: {StatusCode} - {Error}", response.StatusCode, errorContent);
                return "Erro ao gerar resposta da IA";
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response JSON: {Json}", responseJson);
            
            var result = JsonSerializer.Deserialize<GeminiResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var text = result?.Candidates?.FirstOrDefault()
                ?.Content?.Parts?.FirstOrDefault()
                ?.Text?.Trim();
            
            if (string.IsNullOrEmpty(text))
            {
                _logger.LogWarning("Resposta vazia da IA. Response: {Response}", responseJson);
                return "IA não retornou resposta válida";
            }

            _logger.LogInformation("Resposta da IA: {Text}", text);
            return text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao chamar Gemini API");
            return "Erro ao processar requisição de IA";
        }
    }
}

// Models para deserialização da resposta do Gemini
public class GeminiResponse
{
    public List<GeminiCandidate>? Candidates { get; set; }
}

public class GeminiCandidate
{
    public GeminiContent? Content { get; set; }
}

public class GeminiContent
{
    public List<GeminiPart>? Parts { get; set; }
}

public class GeminiPart
{
    public string? Text { get; set; }
}
