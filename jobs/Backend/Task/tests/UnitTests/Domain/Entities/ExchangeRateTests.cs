
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.UnitTests;

public class ExchangeRateTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenSourceCurrencyIsNull()
    {
        var targetCurrency = new Currency("USD");
        var value = 1;

        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null, targetCurrency, value));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTargetCurrencyIsNull()
    {
        var sourceCurrency = new Currency("USD");
        var value = 1;

        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(sourceCurrency, null, value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrow_WhenValueIsNotPositive(int value)
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");

        Assert.Throws<ArgumentException>(() => new ExchangeRate(sourceCurrency, targetCurrency, value));
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 1.2m);

        var result = exchangeRate.ToString();

        Assert.Equal("USD/EUR = 1.2", result);
    }
}