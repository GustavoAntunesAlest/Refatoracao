using LegacyProcs.Models;
using LegacyProcs.Repositories;
using MediatR;

namespace LegacyProcs.Application.Commands;

/// <summary>
/// Command para criar ordem de serviço
/// ✅ CQRS: Separação de comandos
/// </summary>
public record CreateOrdemServicoCommand(
    string Titulo,
    string? Descricao,
    string Tecnico
) : IRequest<int>;

public class CreateOrdemServicoCommandHandler : IRequestHandler<CreateOrdemServicoCommand, int>
{
    private readonly IOrdemServicoRepository _repository;
    private readonly ILogger<CreateOrdemServicoCommandHandler> _logger;

    public CreateOrdemServicoCommandHandler(
        IOrdemServicoRepository repository,
        ILogger<CreateOrdemServicoCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> Handle(CreateOrdemServicoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Criando ordem de serviço: {Titulo}", request.Titulo);

        var ordemServico = new OrdemServico
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Tecnico = request.Tecnico,
            Status = "Aberta",
            DataCriacao = DateTime.Now
        };

        var id = await _repository.CreateAsync(ordemServico);
        
        _logger.LogInformation("Ordem de serviço criada com ID: {Id}", id);
        
        return id;
    }
}
