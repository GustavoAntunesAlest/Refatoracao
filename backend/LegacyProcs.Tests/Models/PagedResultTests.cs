using FluentAssertions;
using LegacyProcs.Models;
using Xunit;

namespace LegacyProcs.Tests.Models;

/// <summary>
/// Testes para PagedResult
/// </summary>
public class PagedResultTests
{
    [Fact]
    public void PagedResult_Should_Calculate_TotalPages_Correctly()
    {
        // Arrange
        var result = new PagedResult<OrdemServico>
        {
            TotalCount = 25,
            PageSize = 10
        };

        // Act & Assert
        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public void PagedResult_Should_Indicate_HasPrevious_Correctly()
    {
        // Arrange
        var result1 = new PagedResult<OrdemServico> { PageNumber = 1 };
        var result2 = new PagedResult<OrdemServico> { PageNumber = 2 };

        // Act & Assert
        result1.HasPrevious.Should().BeFalse();
        result2.HasPrevious.Should().BeTrue();
    }

    [Fact]
    public void PagedResult_Should_Indicate_HasNext_Correctly()
    {
        // Arrange
        var result1 = new PagedResult<OrdemServico> 
        { 
            PageNumber = 1, 
            PageSize = 10, 
            TotalCount = 25 
        };
        var result2 = new PagedResult<OrdemServico> 
        { 
            PageNumber = 3, 
            PageSize = 10, 
            TotalCount = 25 
        };

        // Act & Assert
        result1.HasNext.Should().BeTrue();
        result2.HasNext.Should().BeFalse();
    }

    [Fact]
    public void PagedResult_Should_Initialize_With_Empty_Items()
    {
        // Arrange & Act
        var result = new PagedResult<OrdemServico>();

        // Assert
        result.Items.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }
}
