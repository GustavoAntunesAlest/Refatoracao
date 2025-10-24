using Microsoft.AspNetCore.Mvc;
using LegacyProcs.Models;
using LegacyProcs.Repositories;
using LegacyProcs.Application.Commands;
using LegacyProcs.Application.Queries;
using MediatR;

namespace LegacyProcs.Controllers;

/// <summary>
/// Controller para gerenciar Ordens de Serviço
/// ✅ MIGRADO: .NET 8 com CQRS (MediatR)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdemServicoController : ControllerBase
{
    private readonly IOrdemServicoRepository _repository;
    private readonly IMediator _mediator;
    private readonly ILogger<OrdemServicoController> _logger;

    public OrdemServicoController(
        IOrdemServicoRepository repository,
        IMediator mediator,
        ILogger<OrdemServicoController> logger)
    {
        _repository = repository;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/ordemservico
    /// Lista todas as ordens de serviço (com filtro opcional)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filtro = null)
    {
        try
        {
            _logger.LogInformation("Buscando ordens de serviço. Filtro: {Filtro}", filtro ?? "nenhum");
            var ordens = await _repository.GetAllAsync(filtro);
            return Ok(ordens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar ordens de serviço");
            return StatusCode(500, new { message = "Erro ao buscar ordens de serviço" });
        }
    }

    /// <summary>
    /// GET: api/ordemservico/paged
    /// Lista ordens de serviço com paginação
    /// </summary>
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string? filtro = null)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limite máximo

            _logger.LogInformation("Buscando ordens paginadas. Página: {PageNumber}, Tamanho: {PageSize}", pageNumber, pageSize);
            var result = await _repository.GetPagedAsync(pageNumber, pageSize, filtro);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar ordens paginadas");
            return StatusCode(500, new { message = "Erro ao buscar ordens paginadas" });
        }
    }

    /// <summary>
    /// GET: api/ordemservico/{id}
    /// Obtém uma ordem de serviço por ID
    /// ✅ CQRS: Usando MediatR Query
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetOrdemServicoByIdQuery(id);
            var ordem = await _mediator.Send(query);
            
            if (ordem == null)
            {
                return NotFound(new { message = $"Ordem de serviço {id} não encontrada" });
            }
            
            return Ok(ordem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar ordem de serviço {Id}", id);
            return StatusCode(500, new { message = "Erro ao buscar ordem de serviço" });
        }
    }

    /// <summary>
    /// POST: api/ordemservico
    /// Cria uma nova ordem de serviço
    /// ✅ CQRS: Usando MediatR Command
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrdemServicoCommand command)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, new { id, command.Titulo, command.Descricao, command.Tecnico });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar ordem de serviço");
            return StatusCode(500, new { message = "Erro ao criar ordem de serviço" });
        }
    }

    /// <summary>
    /// PUT: api/ordemservico/{id}
    /// Atualiza uma ordem de serviço
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrdemServico ordemServico)
    {
        try
        {
            if (id != ordemServico.Id)
            {
                return BadRequest(new { message = "ID não corresponde" });
            }

            _logger.LogInformation("Atualizando ordem de serviço ID: {Id}", id);
            var success = await _repository.UpdateAsync(ordemServico);

            if (!success)
            {
                return NotFound(new { message = $"Ordem de serviço {id} não encontrada" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar ordem de serviço {Id}", id);
            return StatusCode(500, new { message = "Erro ao atualizar ordem de serviço" });
        }
    }

    /// <summary>
    /// DELETE: api/ordemservico/{id}
    /// Exclui uma ordem de serviço
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Excluindo ordem de serviço ID: {Id}", id);
            var success = await _repository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Ordem de serviço {id} não encontrada" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir ordem de serviço {Id}", id);
            return StatusCode(500, new { message = "Erro ao excluir ordem de serviço" });
        }
    }
}
