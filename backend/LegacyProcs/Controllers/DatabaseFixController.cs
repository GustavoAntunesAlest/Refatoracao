using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LegacyProcs.Controllers;

/// <summary>
/// Controller temporário para correção de encoding UTF-8
/// ⚠️ REMOVER APÓS EXECUTAR A CORREÇÃO
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DatabaseFixController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseFixController> _logger;

    public DatabaseFixController(IConfiguration configuration, ILogger<DatabaseFixController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// POST: api/databasefix/fix-encoding
    /// Corrige encoding UTF-8 nos dados existentes
    /// Usa mapeamento completo de caracteres acentuados do português
    /// </summary>
    [HttpPost("fix-encoding")]
    public async Task<IActionResult> FixEncoding()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var results = new List<string>();

            // Mapeamento completo de caracteres UTF-8 mal codificados
            // Usando apenas os mais comuns para evitar problemas de encoding no código fonte
            var replacements = new Dictionary<string, string>
            {
                // Vogais minúsculas
                { "Ã£", "ã" },
                { "Ã¡", "á" },
                { "Ã¢", "â" },
                { "Ã ", "à" },
                { "Ã©", "é" },
                { "Ãª", "ê" },
                { "Ã­", "í" },
                { "Ã³", "ó" },
                { "Ã´", "ô" },
                { "Ãµ", "õ" },
                { "Ãº", "ú" },
                { "Ã¼", "ü" },
                
                // Consoantes
                { "Ã§", "ç" },
                { "Ã‡", "Ç" }
            };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Corrigir OrdemServico
                results.Add(await FixColumnEncoding(connection, "OrdemServico", "Tecnico", replacements));
                results.Add(await FixColumnEncoding(connection, "OrdemServico", "Status", replacements));
                results.Add(await FixColumnEncoding(connection, "OrdemServico", "Titulo", replacements));
                results.Add(await FixColumnEncoding(connection, "OrdemServico", "Descricao", replacements));

                // Corrigir Tecnico (se existir)
                try
                {
                    results.Add(await FixColumnEncoding(connection, "Tecnico", "Nome", replacements));
                    results.Add(await FixColumnEncoding(connection, "Tecnico", "Especialidade", replacements));
                }
                catch
                {
                    results.Add("Tabela Tecnico não existe ou não precisa de correção");
                }

                // Corrigir Cliente (se existir)
                try
                {
                    results.Add(await FixColumnEncoding(connection, "Cliente", "RazaoSocial", replacements));
                    results.Add(await FixColumnEncoding(connection, "Cliente", "NomeFantasia", replacements));
                    results.Add(await FixColumnEncoding(connection, "Cliente", "Endereco", replacements));
                    results.Add(await FixColumnEncoding(connection, "Cliente", "Cidade", replacements));
                }
                catch
                {
                    results.Add("Tabela Cliente não existe ou não precisa de correção");
                }
            }

            _logger.LogInformation("Correção de encoding executada com sucesso");

            return Ok(new
            {
                message = "Correção de encoding executada com sucesso!",
                results = results,
                timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar correção de encoding");
            return StatusCode(500, new
            {
                message = "Erro ao executar correção de encoding",
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Corrige encoding de uma coluna específica usando múltiplas substituições
    /// </summary>
    private async Task<string> FixColumnEncoding(
        SqlConnection connection, 
        string tableName, 
        string columnName, 
        Dictionary<string, string> replacements)
    {
        var totalRowsAffected = 0;

        foreach (var replacement in replacements)
        {
            var query = $@"
                UPDATE {tableName} 
                SET {columnName} = REPLACE({columnName}, @oldValue, @newValue) 
                WHERE {columnName} LIKE '%' + @oldValue + '%'";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@oldValue", replacement.Key);
            command.Parameters.AddWithValue("@newValue", replacement.Value);
            
            var rowsAffected = await command.ExecuteNonQueryAsync();
            totalRowsAffected += rowsAffected;
        }

        return $"{tableName}.{columnName}: {totalRowsAffected} registros corrigidos";
    }


    /// <summary>
    /// GET: api/databasefix/verify
    /// Verifica se há dados com encoding incorreto
    /// </summary>
    [HttpGet("verify")]
    public async Task<IActionResult> VerifyEncoding()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var issues = new List<object>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Verificar OrdemServico
                using (var command = new SqlCommand(
                    "SELECT Id, Titulo, Tecnico, Status FROM OrdemServico WHERE Tecnico LIKE '%Ã%' OR Status LIKE '%Ã%' OR Titulo LIKE '%Ã%'", 
                    connection))
                {
                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        issues.Add(new
                        {
                            table = "OrdemServico",
                            id = reader.GetInt32(0),
                            titulo = reader.IsDBNull(1) ? null : reader.GetString(1),
                            tecnico = reader.IsDBNull(2) ? null : reader.GetString(2),
                            status = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }

            return Ok(new
            {
                hasIssues = issues.Count > 0,
                count = issues.Count,
                issues = issues
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar encoding");
            return StatusCode(500, new
            {
                message = "Erro ao verificar encoding",
                error = ex.Message
            });
        }
    }
}
