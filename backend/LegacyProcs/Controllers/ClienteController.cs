using Microsoft.AspNetCore.Mvc;
using LegacyProcs.Models;
using LegacyProcs.Repositories;

namespace LegacyProcs.Controllers;

/// <summary>
/// Controller para gerenciar Clientes
/// ✅ MIGRADO: .NET 8 com Repository Pattern
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteRepository _repository;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(
        IClienteRepository repository,
        ILogger<ClienteController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/cliente
    /// Lista todos os clientes (com busca opcional)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? busca = null)
    {
        try
        {
            _logger.LogInformation("Buscando clientes. Busca: {Busca}", busca ?? "nenhuma");
            var clientes = await _repository.GetAllAsync(busca);
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar clientes");
            return StatusCode(500, new { message = "Erro ao buscar clientes" });
        }
    }

    /// <summary>
    /// GET: api/cliente/{id}
    /// Obtém um cliente por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Buscando cliente ID: {Id}", id);
            var cliente = await _repository.GetByIdAsync(id);
            
            if (cliente == null)
            {
                return NotFound(new { message = $"Cliente {id} não encontrado" });
            }
            
            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar cliente {Id}", id);
            return StatusCode(500, new { message = "Erro ao buscar cliente" });
        }
    }

    /// <summary>
    /// POST: api/cliente
    /// Cria um novo cliente
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Cliente cliente)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            cliente.DataCadastro = DateTime.Now;

            _logger.LogInformation("Criando cliente: {RazaoSocial}", cliente.RazaoSocial);
            var id = await _repository.CreateAsync(cliente);
            cliente.Id = id;

            return CreatedAtAction(nameof(GetById), new { id }, cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente");
            return StatusCode(500, new { message = "Erro ao criar cliente" });
        }
    }

    /// <summary>
    /// PUT: api/cliente/{id}
    /// Atualiza um cliente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
    {
        try
        {
            if (id != cliente.Id)
            {
                return BadRequest(new { message = "ID não corresponde" });
            }

            _logger.LogInformation("Atualizando cliente ID: {Id}", id);
            var success = await _repository.UpdateAsync(cliente);

            if (!success)
            {
                return NotFound(new { message = $"Cliente {id} não encontrado" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente {Id}", id);
            return StatusCode(500, new { message = "Erro ao atualizar cliente" });
        }
    }

    /// <summary>
    /// DELETE: api/cliente/{id}
    /// Exclui um cliente
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Excluindo cliente ID: {Id}", id);
            var success = await _repository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Cliente {id} não encontrado" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir cliente {Id}", id);
            return StatusCode(500, new { message = "Erro ao excluir cliente" });
        }
    }
}
