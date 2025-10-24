using LegacyProcs.Models;

namespace LegacyProcs.Repositories;

/// <summary>
/// Interface para repositório de Técnico
/// ✅ MIGRADO: .NET 8 com Repository Pattern
/// </summary>
public interface ITecnicoRepository
{
    Task<IEnumerable<Tecnico>> GetAllAsync(string? filtro = null);
    Task<IEnumerable<Tecnico>> GetDisponiveisAsync();
    Task<Tecnico?> GetByIdAsync(int id);
    Task<int> CreateAsync(Tecnico tecnico);
    Task<bool> UpdateAsync(Tecnico tecnico);
    Task<bool> DeleteAsync(int id);
}
