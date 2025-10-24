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
/// Testes para TecnicoController
/// </summary>
public class TecnicoControllerTests
{
    private readonly Mock<ITecnicoRepository> _mockRepository;
    private readonly Mock<ILogger<TecnicoController>> _mockLogger;
    private readonly TecnicoController _controller;

    public TecnicoControllerTests()
    {
        _mockRepository = new Mock<ITecnicoRepository>();
        _mockLogger = new Mock<ILogger<TecnicoController>>();
        _controller = new TecnicoController(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_With_List()
    {
        // Arrange
        var tecnicos = new List<Tecnico>
        {
            new() { Id = 1, Nome = "João Silva", Status = "Ativo" },
            new() { Id = 2, Nome = "Maria Santos", Status = "Ativo" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(tecnicos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(tecnicos);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_With_Filtered_List()
    {
        // Arrange
        var tecnicos = new List<Tecnico>
        {
            new() { Id = 1, Nome = "João Silva", Especialidade = "Elétrica", Status = "Ativo" }
        };
        _mockRepository.Setup(r => r.GetAllAsync("Elétrica")).ReturnsAsync(tecnicos);

        // Act
        var result = await _controller.GetAll("Elétrica");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetDisponiveis_Should_Return_Ok_With_Active_Technicians()
    {
        // Arrange
        var tecnicos = new List<Tecnico>
        {
            new() { Id = 1, Nome = "João Silva", Status = "Ativo" },
            new() { Id = 2, Nome = "Maria Santos", Status = "Ativo" }
        };
        _mockRepository.Setup(r => r.GetDisponiveisAsync()).ReturnsAsync(tecnicos);

        // Act
        var result = await _controller.GetDisponiveis();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(tecnicos);
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_When_Found()
    {
        // Arrange
        var tecnico = new Tecnico { Id = 1, Nome = "João Silva", Status = "Ativo" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tecnico);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(tecnico);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Not_Exists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Tecnico?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_Should_Return_CreatedAtAction()
    {
        // Arrange
        var tecnico = new Tecnico { Nome = "Novo Técnico", Especialidade = "TI" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Tecnico>())).ReturnsAsync(1);

        // Act
        var result = await _controller.Create(tecnico);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.ActionName.Should().Be(nameof(TecnicoController.GetById));
    }

    [Fact]
    public async Task Create_Should_Set_Status_To_Ativo()
    {
        // Arrange
        var tecnico = new Tecnico { Nome = "Novo Técnico" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Tecnico>())).ReturnsAsync(1);

        // Act
        await _controller.Create(tecnico);

        // Assert
        tecnico.Status.Should().Be("Ativo");
    }

    [Fact]
    public async Task Update_Should_Return_NoContent_When_Success()
    {
        // Arrange
        var tecnico = new Tecnico { Id = 1, Nome = "Técnico Atualizado", Status = "Ativo" };
        _mockRepository.Setup(r => r.UpdateAsync(tecnico)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, tecnico);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_When_Id_Mismatch()
    {
        // Arrange
        var tecnico = new Tecnico { Id = 2, Nome = "Técnico", Status = "Ativo" };

        // Act
        var result = await _controller.Update(1, tecnico);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_When_Not_Exists()
    {
        // Arrange
        var tecnico = new Tecnico { Id = 1, Nome = "Técnico", Status = "Ativo" };
        _mockRepository.Setup(r => r.UpdateAsync(tecnico)).ReturnsAsync(false);

        // Act
        var result = await _controller.Update(1, tecnico);

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
