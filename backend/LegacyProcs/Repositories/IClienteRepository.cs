using LegacyProcs.Models;

namespace LegacyProcs.Repositories;

/// <summary>
/// Interface para repositório de Cliente
/// ✅ MIGRADO: .NET 8 com Repository Pattern
/// </summary>
public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> GetAllAsync(string? busca = null);
    Task<Cliente?> GetByIdAsync(int id);
    Task<int> CreateAsync(Cliente cliente);
    Task<bool> UpdateAsync(Cliente cliente);
    Task<bool> DeleteAsync(int id);
}
