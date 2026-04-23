using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Domain.Tests.Entities;

public class CurrencyTests
{
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("CZK")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    public void Currency_Constructor_WithValidCodes_SetsCodeCorrectly(string code)
    {
        // Act
        var currency = new Currency(code);

        // Assert
        Assert.Equal(code, currency.Code);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("CZK")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    public void Currency_ToString_ReturnsCode(string code)
    {
        // Arrange
        var currency = new Currency(code);

        // Act
        var result = currency.ToString();

        // Assert
        Assert.Equal(code, result);
    }
}
