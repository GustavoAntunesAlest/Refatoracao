using FluentAssertions;
using LegacyProcs.Controllers;
using LegacyProcs.Models;
using LegacyProcs.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LegacyProcs.Tests.Controllers;

/// <summary>
/// Testes para ClienteController
/// </summary>
public class ClienteControllerTests
{
    private readonly Mock<IClienteRepository> _mockRepository;
    private readonly Mock<ILogger<ClienteController>> _mockLogger;
    private readonly ClienteController _controller;

    public ClienteControllerTests()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _mockLogger = new Mock<ILogger<ClienteController>>();
        _controller = new ClienteController(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_With_List()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new() { Id = 1, RazaoSocial = "Empresa A", CNPJ = "11111111000111" },
            new() { Id = 2, RazaoSocial = "Empresa B", CNPJ = "22222222000122" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(clientes);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(clientes);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_With_Filtered_List()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new() { Id = 1, RazaoSocial = "Empresa Teste", CNPJ = "11111111000111" }
        };
        _mockRepository.Setup(r => r.GetAllAsync("Teste")).ReturnsAsync(clientes);

        // Act
        var result = await _controller.GetAll("Teste");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_When_Found()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, RazaoSocial = "Empresa", CNPJ = "11111111000111" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(cliente);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Not_Exists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Cliente?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_Should_Return_CreatedAtAction()
    {
        // Arrange
        var cliente = new Cliente { RazaoSocial = "Nova Empresa", CNPJ = "11111111000111" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Cliente>())).ReturnsAsync(1);

        // Act
        var result = await _controller.Create(cliente);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.ActionName.Should().Be(nameof(ClienteController.GetById));
    }

    [Fact]
    public async Task Update_Should_Return_NoContent_When_Success()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, RazaoSocial = "Empresa Atualizada", CNPJ = "11111111000111" };
        _mockRepository.Setup(r => r.UpdateAsync(cliente)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, cliente);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_When_Id_Mismatch()
    {
        // Arrange
        var cliente = new Cliente { Id = 2, RazaoSocial = "Empresa", CNPJ = "11111111000111" };

        // Act
        var result = await _controller.Update(1, cliente);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_When_Not_Exists()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, RazaoSocial = "Empresa", CNPJ = "11111111000111" };
        _mockRepository.Setup(r => r.UpdateAsync(cliente)).ReturnsAsync(false);

        // Act
        var result = await _controller.Update(1, cliente);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_Should_Return_NoContent_When_Success()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_When_Not_Exists()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}
