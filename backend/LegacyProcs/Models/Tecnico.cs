namespace LegacyProcs.Models;

/// <summary>
/// Modelo de Técnico
/// ✅ MIGRADO: .NET 8 com nullable reference types
/// </summary>
public class Tecnico
{
    public int Id { get; set; }
    
    public string Nome { get; set; } = string.Empty;
    
    public string? Email { get; set; }
    
    public string? Telefone { get; set; }
    
    public string? Especialidade { get; set; }
    
    public string Status { get; set; } = "Ativo"; // "Ativo", "Inativo", "Férias"
    
    public DateTime DataCadastro { get; set; }
}
