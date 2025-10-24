using FluentAssertions;
using LegacyProcs.Models;
using Xunit;

namespace LegacyProcs.Tests.Models;

/// <summary>
/// Testes para modelo OrdemServico
/// </summary>
public class OrdemServicoTests
{
    [Fact]
    public void OrdemServico_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var os = new OrdemServico();

        // Assert
        os.Id.Should().Be(0);
        os.Titulo.Should().BeEmpty();
        os.Tecnico.Should().BeEmpty();
        os.Status.Should().BeEmpty();
        os.Descricao.Should().BeNull();
        os.DataAtualizacao.Should().BeNull();
    }

    [Fact]
    public void OrdemServico_Should_Set_Properties_Correctly()
    {
        // Arrange & Act
        var os = new OrdemServico
        {
            Id = 1,
            Titulo = "Manutenção",
            Descricao = "Manutenção preventiva",
            Tecnico = "João Silva",
            Status = "Aberta",
            DataCriacao = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };

        // Assert
        os.Id.Should().Be(1);
        os.Titulo.Should().Be("Manutenção");
        os.Descricao.Should().Be("Manutenção preventiva");
        os.Tecnico.Should().Be("João Silva");
        os.Status.Should().Be("Aberta");
        os.DataCriacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        os.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void OrdemServico_Should_Allow_Null_Optional_Fields()
    {
        // Arrange & Act
        var os = new OrdemServico
        {
            Titulo = "Teste",
            Tecnico = "João",
            Status = "Aberta"
        };

        // Assert
        os.Descricao.Should().BeNull();
        os.DataAtualizacao.Should().BeNull();
    }

    [Theory]
    [InlineData("Aberta")]
    [InlineData("Em Andamento")]
    [InlineData("Concluída")]
    [InlineData("Cancelada")]
    public void OrdemServico_Should_Accept_Different_Status_Values(string status)
    {
        // Arrange & Act
        var os = new OrdemServico { Status = status };

        // Assert
        os.Status.Should().Be(status);
    }
}
