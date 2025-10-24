using FluentAssertions;
using LegacyProcs.Models;
using Xunit;

namespace LegacyProcs.Tests.Models;

/// <summary>
/// Testes para modelo Tecnico
/// </summary>
public class TecnicoTests
{
    [Fact]
    public void Tecnico_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var tecnico = new Tecnico();

        // Assert
        tecnico.Id.Should().Be(0);
        tecnico.Nome.Should().BeEmpty();
        tecnico.Status.Should().Be("Ativo");
    }

    [Fact]
    public void Tecnico_Should_Set_Properties_Correctly()
    {
        // Arrange & Act
        var tecnico = new Tecnico
        {
            Id = 1,
            Nome = "João Silva",
            Email = "joao@teste.com",
            Telefone = "(11) 98765-4321",
            Especialidade = "Elétrica",
            Status = "Ativo",
            DataCadastro = DateTime.Now
        };

        // Assert
        tecnico.Id.Should().Be(1);
        tecnico.Nome.Should().Be("João Silva");
        tecnico.Email.Should().Be("joao@teste.com");
        tecnico.Especialidade.Should().Be("Elétrica");
        tecnico.Status.Should().Be("Ativo");
    }

    [Theory]
    [InlineData("Ativo")]
    [InlineData("Inativo")]
    [InlineData("Férias")]
    public void Tecnico_Should_Accept_Different_Status_Values(string status)
    {
        // Arrange & Act
        var tecnico = new Tecnico { Status = status };

        // Assert
        tecnico.Status.Should().Be(status);
    }
}
