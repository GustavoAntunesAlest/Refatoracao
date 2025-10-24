using LegacyProcs.Models;

namespace LegacyProcs.Repositories;

/// <summary>
/// Interface para repositório de Ordem de Serviço
/// ✅ MIGRADO: .NET 8 com Repository Pattern
/// </summary>
public interface IOrdemServicoRepository
{
    /// <summary>
    /// Busca todas as ordens de serviço com filtro opcional
    /// </summary>
    Task<IEnumerable<OrdemServico>> GetAllAsync(string? filtro = null);

    /// <summary>
    /// Busca ordem de serviço por ID
    /// </summary>
    Task<OrdemServico?> GetByIdAsync(int id);

    /// <summary>
    /// Cria nova ordem de serviço
    /// </summary>
    Task<int> CreateAsync(OrdemServico ordemServico);

    /// <summary>
    /// Atualiza ordem de serviço
    /// </summary>
    Task<bool> UpdateAsync(OrdemServico ordemServico);

    /// <summary>
    /// Exclui ordem de serviço
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca ordens de serviço com paginação
    /// </summary>
    Task<PagedResult<OrdemServico>> GetPagedAsync(int pageNumber, int pageSize, string? filtro = null);
}
