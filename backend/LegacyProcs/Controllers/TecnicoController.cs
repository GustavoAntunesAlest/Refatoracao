using Microsoft.AspNetCore.Mvc;
using LegacyProcs.Models;
using LegacyProcs.Repositories;

namespace LegacyProcs.Controllers;

/// <summary>
/// Controller para gerenciar Técnicos
/// ✅ MIGRADO: .NET 8 com Repository Pattern
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TecnicoController : ControllerBase
{
    private readonly ITecnicoRepository _repository;
    private readonly ILogger<TecnicoController> _logger;

    public TecnicoController(
        ITecnicoRepository repository,
        ILogger<TecnicoController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/tecnico
    /// Lista todos os técnicos (com filtro opcional)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filtro = null)
    {
        try
        {
            _logger.LogInformation("Buscando técnicos. Filtro: {Filtro}", filtro ?? "nenhum");
            var tecnicos = await _repository.GetAllAsync(filtro);
            return Ok(tecnicos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar técnicos");
            return StatusCode(500, new { message = "Erro ao buscar técnicos" });
        }
    }

    /// <summary>
    /// GET: api/tecnico/disponiveis
    /// Lista técnicos disponíveis (status Ativo)
    /// </summary>
    [HttpGet("disponiveis")]
    public async Task<IActionResult> GetDisponiveis()
    {
        try
        {
            _logger.LogInformation("Buscando técnicos disponíveis");
            var tecnicos = await _repository.GetDisponiveisAsync();
            return Ok(tecnicos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar técnicos disponíveis");
            return StatusCode(500, new { message = "Erro ao buscar técnicos disponíveis" });
        }
    }

    /// <summary>
    /// GET: api/tecnico/{id}
    /// Obtém um técnico por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Buscando técnico ID: {Id}", id);
            var tecnico = await _repository.GetByIdAsync(id);
            
            if (tecnico == null)
            {
                return NotFound(new { message = $"Técnico {id} não encontrado" });
            }
            
            return Ok(tecnico);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar técnico {Id}", id);
            return StatusCode(500, new { message = "Erro ao buscar técnico" });
        }
    }

    /// <summary>
    /// POST: api/tecnico
    /// Cria um novo técnico
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Tecnico tecnico)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tecnico.DataCadastro = DateTime.Now;
            tecnico.Status = "Ativo";

            _logger.LogInformation("Criando técnico: {Nome}", tecnico.Nome);
            var id = await _repository.CreateAsync(tecnico);
            tecnico.Id = id;

            return CreatedAtAction(nameof(GetById), new { id }, tecnico);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar técnico");
            return StatusCode(500, new { message = "Erro ao criar técnico" });
        }
    }

    /// <summary>
    /// PUT: api/tecnico/{id}
    /// Atualiza um técnico
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Tecnico tecnico)
    {
        try
        {
            if (id != tecnico.Id)
            {
                return BadRequest(new { message = "ID não corresponde" });
            }

            _logger.LogInformation("Atualizando técnico ID: {Id}", id);
            var success = await _repository.UpdateAsync(tecnico);

            if (!success)
            {
                return NotFound(new { message = $"Técnico {id} não encontrado" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar técnico {Id}", id);
            return StatusCode(500, new { message = "Erro ao atualizar técnico" });
        }
    }

    /// <summary>
    /// DELETE: api/tecnico/{id}
    /// Exclui um técnico
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Excluindo técnico ID: {Id}", id);
            var success = await _repository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Técnico {id} não encontrado" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir técnico {Id}", id);
            return StatusCode(500, new { message = "Erro ao excluir técnico" });
        }
    }
}
