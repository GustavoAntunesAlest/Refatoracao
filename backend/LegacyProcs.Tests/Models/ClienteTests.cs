using FluentAssertions;
using LegacyProcs.Models;
using Xunit;

namespace LegacyProcs.Tests.Models;

/// <summary>
/// Testes para modelo Cliente
/// </summary>
public class ClienteTests
{
    [Fact]
    public void Cliente_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var cliente = new Cliente();

        // Assert
        cliente.Id.Should().Be(0);
        cliente.RazaoSocial.Should().BeEmpty();
        cliente.CNPJ.Should().BeEmpty();
    }

    [Fact]
    public void Cliente_Should_Set_Properties_Correctly()
    {
        // Arrange & Act
        var cliente = new Cliente
        {
            Id = 1,
            RazaoSocial = "Empresa Teste LTDA",
            NomeFantasia = "Teste",
            CNPJ = "12.345.678/0001-90",
            Email = "contato@teste.com",
            Telefone = "(11) 1234-5678",
            Endereco = "Rua Teste, 123",
            Cidade = "São Paulo",
            Estado = "SP",
            CEP = "01234-567",
            DataCadastro = DateTime.Now
        };

        // Assert
        cliente.Id.Should().Be(1);
        cliente.RazaoSocial.Should().Be("Empresa Teste LTDA");
        cliente.CNPJ.Should().Be("12.345.678/0001-90");
        cliente.Email.Should().Be("contato@teste.com");
    }

    [Fact]
    public void Cliente_Should_Allow_Null_Optional_Fields()
    {
        // Arrange & Act
        var cliente = new Cliente
        {
            RazaoSocial = "Empresa",
            CNPJ = "12345678000190"
        };

        // Assert
        cliente.NomeFantasia.Should().BeNull();
        cliente.Email.Should().BeNull();
        cliente.Telefone.Should().BeNull();
    }
}
