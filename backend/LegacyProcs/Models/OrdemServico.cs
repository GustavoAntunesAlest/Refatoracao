namespace LegacyProcs.Models;

/// <summary>
/// Modelo de Ordem de Serviço
/// ✅ MIGRADO: .NET 8 com nullable reference types
/// </summary>
public class OrdemServico
{
    public int Id { get; set; }
    
    public string Titulo { get; set; } = string.Empty;
    
    public string? Descricao { get; set; }
    
    public string Tecnico { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public DateTime DataCriacao { get; set; }
    
    public DateTime? DataAtualizacao { get; set; }
}
