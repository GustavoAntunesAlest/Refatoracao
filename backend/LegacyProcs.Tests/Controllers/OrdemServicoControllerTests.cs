using FluentAssertions;
using LegacyProcs.Controllers;
using LegacyProcs.Models;
using LegacyProcs.Repositories;
using LegacyProcs.Application.Commands;
using LegacyProcs.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Moq;
using Xunit;

namespace LegacyProcs.Tests.Controllers;

/// <summary>
/// Testes para OrdemServicoController
/// </summary>
public class OrdemServicoControllerTests
{
    private readonly Mock<IOrdemServicoRepository> _mockRepository;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<OrdemServicoController>> _mockLogger;
    private readonly OrdemServicoController _controller;

    public OrdemServicoControllerTests()
    {
        _mockRepository = new Mock<IOrdemServicoRepository>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<OrdemServicoController>>();
        _controller = new OrdemServicoController(_mockRepository.Object, _mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_With_List()
    {
        // Arrange
        var ordens = new List<OrdemServico>
        {
            new() { Id = 1, Titulo = "OS 1", Tecnico = "João", Status = "Aberta" },
            new() { Id = 2, Titulo = "OS 2", Tecnico = "Maria", Status = "Concluída" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(ordens);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(ordens);
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_When_Found()
    {
        // Arrange
        var ordem = new OrdemServico { Id = 1, Titulo = "Teste", Tecnico = "João", Status = "Aberta" };
        _mockMediator.Setup(m => m.Send(It.IsAny<GetOrdemServicoByIdQuery>(), default)).ReturnsAsync(ordem);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(ordem);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Not_Exists()
    {
        // Arrange
        _mockMediator.Setup(m => m.Send(It.IsAny<GetOrdemServicoByIdQuery>(), default)).ReturnsAsync((OrdemServico?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_Should_Return_CreatedAtAction()
    {
        // Arrange
        var command = new CreateOrdemServicoCommand("Nova OS", "Descrição", "João");
        _mockMediator.Setup(m => m.Send(It.IsAny<CreateOrdemServicoCommand>(), default)).ReturnsAsync(1);

        // Act
        var result = await _controller.Create(command);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.ActionName.Should().Be(nameof(OrdemServicoController.GetById));
    }

    [Fact]
    public async Task Update_Should_Return_NoContent_When_Success()
    {
        // Arrange
        var ordem = new OrdemServico { Id = 1, Titulo = "OS Atualizada", Tecnico = "João", Status = "Em Andamento" };
        _mockRepository.Setup(r => r.UpdateAsync(ordem)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, ordem);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_Return_BadRequest_When_Id_Mismatch()
    {
        // Arrange
        var ordem = new OrdemServico { Id = 2, Titulo = "OS", Tecnico = "João", Status = "Aberta" };

        // Act
        var result = await _controller.Update(1, ordem);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
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
