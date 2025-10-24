using Microsoft.EntityFrameworkCore;
using LegacyProcs.Models;

namespace LegacyProcs.Data;

/// <summary>
/// DbContext do Entity Framework Core
/// ✅ EF Core 8: Substituindo ADO.NET mantendo compatibilidade 100%
/// </summary>
public class LegacyProcsDbContext : DbContext
{
    public LegacyProcsDbContext(DbContextOptions<LegacyProcsDbContext> options)
        : base(options)
    {
    }

    public DbSet<OrdemServico> OrdemServico { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Tecnico> Tecnico { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da tabela OrdemServico
        // ✅ IMPORTANTE: Mantém estrutura EXATA do banco existente
        modelBuilder.Entity<OrdemServico>(entity =>
        {
            entity.ToTable("OrdemServico");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Descricao)
                .HasMaxLength(1000);
            
            entity.Property(e => e.Tecnico)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.DataCriacao)
                .IsRequired();
            
            entity.Property(e => e.DataAtualizacao);
        });

        // Configuração da tabela Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.RazaoSocial)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(200);
            
            entity.Property(e => e.CNPJ)
                .IsRequired()
                .HasMaxLength(18);
            
            entity.Property(e => e.Email)
                .HasMaxLength(100);
            
            entity.Property(e => e.Telefone)
                .HasMaxLength(20);
            
            entity.Property(e => e.Endereco)
                .HasMaxLength(300);
            
            entity.Property(e => e.Cidade)
                .HasMaxLength(100);
            
            entity.Property(e => e.Estado)
                .HasMaxLength(2);
            
            entity.Property(e => e.CEP)
                .HasMaxLength(10);
            
            entity.Property(e => e.DataCadastro)
                .IsRequired();
        });

        // Configuração da tabela Tecnico
        modelBuilder.Entity<Tecnico>(entity =>
        {
            entity.ToTable("Tecnico");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Email)
                .HasMaxLength(100);
            
            entity.Property(e => e.Telefone)
                .HasMaxLength(20);
            
            entity.Property(e => e.Especialidade)
                .HasMaxLength(100);
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(e => e.DataCadastro)
                .IsRequired();
        });
    }
}
