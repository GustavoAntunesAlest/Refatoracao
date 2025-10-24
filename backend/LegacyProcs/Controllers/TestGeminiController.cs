using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LegacyProcs.Controllers;

/// <summary>
/// Controller de teste para diagnosticar problemas com Gemini API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestGeminiController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TestGeminiController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public TestGeminiController(
        IConfiguration configuration,
        ILogger<TestGeminiController> logger,
        IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Lista todos os modelos disponíveis para a API Key
    /// </summary>
    [HttpGet("list-models")]
    public async Task<IActionResult> ListModels()
    {
        try
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var httpClient = _httpClientFactory.CreateClient("Gemini");
            
            var url = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
            
            _logger.LogInformation("Listando modelos disponíveis...");
            
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro ao listar modelos: {StatusCode} - {Content}", response.StatusCode, content);
                return StatusCode((int)response.StatusCode, new { error = content });
            }
            
            var jsonDoc = JsonDocument.Parse(content);
            var models = jsonDoc.RootElement.GetProperty("models");
            
            var modelList = new List<object>();
            foreach (var model in models.EnumerateArray())
            {
                var name = model.GetProperty("name").GetString();
                var supportedMethods = new List<string>();
                
                if (model.TryGetProperty("supportedGenerationMethods", out var methods))
                {
                    foreach (var method in methods.EnumerateArray())
                    {
                        supportedMethods.Add(method.GetString() ?? "");
                    }
                }
                
                modelList.Add(new
                {
                    name = name?.Replace("models/", ""),
                    fullName = name,
                    supportedMethods = supportedMethods,
                    supportsGenerateContent = supportedMethods.Contains("generateContent")
                });
            }
            
            return Ok(new
            {
                totalModels = modelList.Count,
                models = modelList,
                recommendation = modelList.FirstOrDefault(m => 
                    ((dynamic)m).supportsGenerateContent == true)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar modelos");
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    /// <summary>
    /// Testa um modelo específico
    /// </summary>
    [HttpPost("test-model")]
    public async Task<IActionResult> TestModel([FromBody] TestModelRequest request)
    {
        try
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var httpClient = _httpClientFactory.CreateClient("Gemini");
            
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{request.ModelName}:generateContent?key={apiKey}";
            
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = "Olá, você está funcionando?" }
                        }
                    }
                }
            };
            
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            _logger.LogInformation("Testando modelo: {Model}", request.ModelName);
            
            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            return Ok(new
            {
                model = request.ModelName,
                statusCode = (int)response.StatusCode,
                success = response.IsSuccessStatusCode,
                response = responseContent
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao testar modelo");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class TestModelRequest
{
    public string ModelName { get; set; } = string.Empty;
}
