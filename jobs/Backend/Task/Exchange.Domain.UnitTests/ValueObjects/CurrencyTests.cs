using Exchange.Domain.ValueObjects;
using FluentAssertions;

namespace Exchange.Domain.UnitTests.ValueObjects;

public class CurrencyTests
{
    [Fact]
    public void FromCode_WhenCodeIsValid_ReturnsCurrency()
    {
        // Arrange
        const string code = "EUR";

        // Act
        var currency = Currency.FromCode(code);

        // Assert
        currency.Should().Be(Currency.EUR);
    }

    [Fact]
    public void FromCode_WhenCodeIsInvalid_ThrowsException()
    {
        // Arrange
        const string code = "INVALID";

        // Act
        var act = () => Currency.FromCode(code);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FromCode_WhenCodeIsEmpty_ThenThrowsException()
    {
        // Arrange
        const string code = "";

        // Act
        var act = () => Currency.FromCode(code);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}