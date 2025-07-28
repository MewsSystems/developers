using Mews.ExchangeRateUpdater.Domain.ValueObjects;

namespace Mews.ExchangeRateUpdater.Domain.UnitTests;

public class CurrencyTests
{
    [Theory]
    [InlineData("eur", "EUR")]
    [InlineData("Usd", "USD")]
    [InlineData("GBP", "GBP")]
    public void Constructor_ShouldNormalizeCode_WhenCodeIsValid(string input, string expected)
    {
        // Act
        var currency = new Currency(input);

        // Assert
        Assert.Equal(expected, currency.Code);
    }

    [Theory]
    [InlineData("GBP", "gbp")]
    [InlineData("usd", "USD")]
    public void Equals_ShouldReturnTrue_WhenCodesAreEqual(string code1, string code2)
    {
        // Arrange
        var c1 = new Currency(code1);
        var c2 = new Currency(code2);

        // Assert
        Assert.Equal(c1, c2);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenCodesAreDifferent()
    {
        // Arrange
        var c1 = new Currency("JPY");
        var c2 = new Currency("AUD");

        // Assert
        Assert.NotEqual(c1, c2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("eu")]
    [InlineData("EURO")]
    public void Constructor_ShouldThrow_WhenCodeLengthIsInvalid(string code)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(code));
    }

    [Theory]
    [InlineData("1US")]
    [InlineData("US$")]
    [InlineData("EU€")]
    public void Constructor_ShouldThrow_WhenCodeContainsInvalidCharacters(string code)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(code));
    }
    
}