using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Tests.Models;

public class CurrencyTests
{
    [Fact]
    public void Currency_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        const string code = "USD";

        // Act
        var currency = new Currency(code);

        // Assert
        Assert.Equal(code, currency.Code);
    }

    [Fact]
    public void Currency_WithEmptyCode_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(""));
        Assert.Throws<ArgumentException>(() => new Currency("   "));
        Assert.Throws<ArgumentException>(() => new Currency(null!));
    }

    [Fact]
    public void Currency_WithInvalidLength_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency("US")); // Too short
        Assert.Throws<ArgumentException>(() => new Currency("USDD")); // Too long
    }

    [Fact]
    public void Currency_ShouldConvertToUpperCase()
    {
        // Arrange & Act
        var currency = new Currency("usd");

        // Assert
        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Currency_Equals_ShouldReturnTrue_WhenSameCode()
    {
        // Arrange
        var currency1 = new Currency("USD");
        var currency2 = new Currency("USD");

        // Act & Assert
        Assert.Equal(currency1, currency2);
        Assert.True(currency1.Equals(currency2));
    }

    [Fact]
    public void Currency_Equals_ShouldReturnFalse_WhenDifferentCode()
    {
        // Arrange
        var currency1 = new Currency("USD");
        var currency2 = new Currency("EUR");

        // Act & Assert
        Assert.NotEqual(currency1, currency2);
        Assert.False(currency1.Equals(currency2));
    }

    [Fact]
    public void Currency_GetHashCode_ShouldReturnSameValue_WhenSameCode()
    {
        // Arrange
        var currency1 = new Currency("USD");
        var currency2 = new Currency("USD");

        // Act & Assert
        Assert.Equal(currency1.GetHashCode(), currency2.GetHashCode());
    }

    [Fact]
    public void Currency_ToString_ShouldReturnCode()
    {
        // Arrange
        var currency = new Currency("USD");

        // Act
        var result = currency.ToString();

        // Assert
        Assert.Equal("USD", result);
    }
} 