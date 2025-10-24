using LegacyProcs.Models;
using LegacyProcs.Repositories;
using MediatR;

namespace LegacyProcs.Application.Queries;

/// <summary>
/// Query para buscar ordem de serviço por ID
/// ✅ CQRS: Separação de queries
/// </summary>
public record GetOrdemServicoByIdQuery(int Id) : IRequest<OrdemServico?>;

public class GetOrdemServicoByIdQueryHandler : IRequestHandler<GetOrdemServicoByIdQuery, OrdemServico?>
{
    private readonly IOrdemServicoRepository _repository;
    private readonly ILogger<GetOrdemServicoByIdQueryHandler> _logger;

    public GetOrdemServicoByIdQueryHandler(
        IOrdemServicoRepository repository,
        ILogger<GetOrdemServicoByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OrdemServico?> Handle(GetOrdemServicoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando ordem de serviço ID: {Id}", request.Id);
        
        var ordem = await _repository.GetByIdAsync(request.Id);
        
        if (ordem == null)
        {
            _logger.LogWarning("Ordem de serviço {Id} não encontrada", request.Id);
        }
        
        return ordem;
    }
}
