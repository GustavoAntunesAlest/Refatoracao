namespace LegacyProcs.Models;

/// <summary>
/// Modelo de Cliente
/// ✅ MIGRADO: .NET 8 com nullable reference types
/// </summary>
public class Cliente
{
    public int Id { get; set; }
    
    public string RazaoSocial { get; set; } = string.Empty;
    
    public string? NomeFantasia { get; set; }
    
    public string CNPJ { get; set; } = string.Empty;
    
    public string? Email { get; set; }
    
    public string? Telefone { get; set; }
    
    public string? Endereco { get; set; }
    
    public string? Cidade { get; set; }
    
    public string? Estado { get; set; }
    
    public string? CEP { get; set; }
    
    public DateTime DataCadastro { get; set; }
}
