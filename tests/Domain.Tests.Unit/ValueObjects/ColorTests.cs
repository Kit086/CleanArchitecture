using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.ValueObjects;
using FluentAssertions;

namespace CleanArchitecture.Domain.Tests.Unit.ValueObjects;

public class ColorTests
{
    [Fact]
    public void From_ShouldReturnsCorrectColorCode()
    {
        // Arrange
        var code = "#FFFFFF";
        
        // Act
        var color = Colour.From(code);
        
        // Assert
        color.Code.Should().Be(code);
    }

    [Fact]
    public void ToString_ShouldReturnsCode()
    {
        // Arrange
        // Act
        var color = Colour.White;

        // Assert
        color.ToString().Should().Be(color.Code);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldPerformImplicitConversionToColorCodeString()
    {
        // Arrange
        // Act
        string code = Colour.White;

        // Assert
        code.Should().Be("#FFFFFF");
    }

    [Fact]
    void ExplicitConversion_ShouldPerformExplicitConversion_WhenGivenSupportedColorCode()
    {
        // Arrange
        // Act
        var color = (Colour)"#FFFFFF";

        // Assert
        color.Should().Be(Colour.White);
    }

    [Fact]
    public void From_ShouldThrowUnsupportedColourException_WhenGivenNotSupportedColorCode()
    {
        FluentActions.Invoking(() => Colour.From("##FF33CC"))
            .Should().Throw<UnsupportedColourException>();
    }
}