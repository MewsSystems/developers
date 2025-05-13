using ExchangeRateUpdater.Entities;
using Shouldly;

namespace ExchangeRateUpdater.Tests.Entities;

public class ExchangeRateTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        Currency sourceCurrency = new("USD");
        Currency targetCurrency = new("EUR");
        decimal value = 1.25m;

        // Act
        ExchangeRate exchangeRate = new(sourceCurrency, targetCurrency, value);

        // Assert
        exchangeRate.SourceCurrency.ShouldBe(sourceCurrency);
        exchangeRate.TargetCurrency.ShouldBe(targetCurrency);
        exchangeRate.Value.ShouldBe(value);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        // Arrange
        Currency sourceCurrency = new("USD");
        Currency targetCurrency = new("EUR");
        decimal value = 1.25m;
        ExchangeRate exchangeRate = new(sourceCurrency, targetCurrency, value);

        // Act
        string result = exchangeRate.ToString();

        // Assert
        result.ShouldBe("USD/EUR=1.25");
    }
}
