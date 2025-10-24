using Microsoft.AspNetCore.Mvc;
using LegacyProcs.Services.AI;
using LegacyProcs.Repositories;

namespace LegacyProcs.Controllers;

/// <summary>
/// Controller para funcionalidades de IA
/// ✨ NOVA FEATURE - Assistente inteligente para ordens de serviço
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IAAssistenteController : ControllerBase
{
    private readonly IGeminiService _geminiService;
    private readonly ILogger<IAAssistenteController> _logger;
    private readonly ITecnicoRepository _tecnicoRepository;

    public IAAssistenteController(
        IGeminiService geminiService,
        ILogger<IAAssistenteController> logger,
        ITecnicoRepository tecnicoRepository)
    {
        _geminiService = geminiService;
        _logger = logger;
        _tecnicoRepository = tecnicoRepository;
    }

    /// <summary>
    /// POST: api/iaassistente/gerar-descricao
    /// Gera descrição automática baseada no título da ordem de serviço
    /// </summary>
    /// <param name="request">Objeto contendo o título</param>
    /// <returns>Descrição gerada pela IA</returns>
    [HttpPost("gerar-descricao")]
    public async Task<IActionResult> GerarDescricao([FromBody] GerarDescricaoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Titulo))
            {
                return BadRequest(new { message = "Título é obrigatório" });
            }

            _logger.LogInformation("🤖 Gerando descrição para: {Titulo}", request.Titulo);
            var descricao = await _geminiService.GerarDescricaoAsync(request.Titulo);

            return Ok(new { descricao });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar descrição com IA");
            return StatusCode(500, new { message = "Erro ao gerar descrição com IA" });
        }
    }

    /// <summary>
    /// POST: api/iaassistente/sugerir-tecnico
    /// Sugere técnico disponível baseado na descrição do problema
    /// </summary>
    /// <param name="request">Objeto contendo a descrição</param>
    /// <returns>Nome do técnico sugerido pela IA</returns>
    [HttpPost("sugerir-tecnico")]
    public async Task<IActionResult> SugerirTecnico([FromBody] SugerirTecnicoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Descricao))
            {
                return BadRequest(new { message = "Descrição é obrigatória" });
            }

            _logger.LogInformation("🤖 Buscando técnicos disponíveis...");
            
            // Buscar todos os técnicos do banco
            var todosTecnicos = await _tecnicoRepository.GetAllAsync();
            
            // Filtrar apenas técnicos ATIVOS (não de férias, não inativos)
            var tecnicosDisponiveis = todosTecnicos
                .Where(t => t.Status.Equals("Ativo", StringComparison.OrdinalIgnoreCase))
                .Select(t => $"{t.Nome} ({t.Especialidade ?? "Geral"})")
                .ToList();
            
            if (!tecnicosDisponiveis.Any())
            {
                _logger.LogWarning("Nenhum técnico disponível no momento");
                return Ok(new { especialidade = "Nenhum técnico disponível no momento" });
            }
            
            _logger.LogInformation("🤖 {Count} técnicos disponíveis. Sugerindo melhor opção...", tecnicosDisponiveis.Count);
            var tecnicoSugerido = await _geminiService.SugerirTecnicoAsync(request.Descricao, tecnicosDisponiveis);

            // Verificar se a IA respondeu que não há técnico adequado
            if (tecnicoSugerido.Equals("NENHUM", StringComparison.OrdinalIgnoreCase) || 
                tecnicoSugerido.Contains("nenhum", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("IA não encontrou técnico adequado para o serviço");
                return Ok(new { especialidade = "Nenhum técnico disponível com especialidade adequada para este serviço" });
            }
            
            // Verificar se a IA retornou um técnico válido do banco
            var tecnicoEncontrado = todosTecnicos.Any(t => 
                tecnicoSugerido.Contains(t.Nome, StringComparison.OrdinalIgnoreCase));
            
            if (!tecnicoEncontrado)
            {
                _logger.LogWarning("IA retornou técnico não encontrado no banco. Resposta: {Resposta}", tecnicoSugerido);
                return Ok(new { especialidade = "Nenhum técnico disponível com especialidade adequada para este serviço" });
            }

            _logger.LogInformation("✅ Técnico sugerido: {Tecnico}", tecnicoSugerido);
            return Ok(new { especialidade = tecnicoSugerido });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao sugerir técnico com IA");
            return StatusCode(500, new { message = "Erro ao sugerir técnico com IA" });
        }
    }

    /// <summary>
    /// POST: api/iaassistente/analisar-prioridade
    /// Analisa e classifica a prioridade da ordem de serviço
    /// </summary>
    /// <param name="request">Objeto contendo a descrição</param>
    /// <returns>Prioridade classificada pela IA</returns>
    [HttpPost("analisar-prioridade")]
    public async Task<IActionResult> AnalisarPrioridade([FromBody] AnalisarPrioridadeRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Descricao))
            {
                return BadRequest(new { message = "Descrição é obrigatória" });
            }

            _logger.LogInformation("🤖 Analisando prioridade");
            var prioridade = await _geminiService.AnalisarPrioridadeAsync(request.Descricao);

            return Ok(new { prioridade });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao analisar prioridade com IA");
            return StatusCode(500, new { message = "Erro ao analisar prioridade com IA" });
        }
    }

    /// <summary>
    /// POST: api/iaassistente/estimar-tempo
    /// Estima tempo necessário para conclusão do serviço
    /// </summary>
    /// <param name="request">Objeto contendo a descrição</param>
    /// <returns>Estimativa de tempo gerada pela IA</returns>
    [HttpPost("estimar-tempo")]
    public async Task<IActionResult> EstimarTempo([FromBody] EstimarTempoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Descricao))
            {
                return BadRequest(new { message = "Descrição é obrigatória" });
            }

            _logger.LogInformation("🤖 Estimando tempo de conclusão");
            var tempo = await _geminiService.EstimarTempoAsync(request.Descricao);

            return Ok(new { tempo });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao estimar tempo com IA");
            return StatusCode(500, new { message = "Erro ao estimar tempo com IA" });
        }
    }
}

// DTOs (Data Transfer Objects)
public class GerarDescricaoRequest
{
    public string Titulo { get; set; } = string.Empty;
}

public class SugerirTecnicoRequest
{
    public string Descricao { get; set; } = string.Empty;
}

public class AnalisarPrioridadeRequest
{
    public string Descricao { get; set; } = string.Empty;
}

public class EstimarTempoRequest
{
    public string Descricao { get; set; } = string.Empty;
}
