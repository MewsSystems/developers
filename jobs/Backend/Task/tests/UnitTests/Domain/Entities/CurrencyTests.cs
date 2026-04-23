using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.UnitTests;

public class CurrencyTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenCodeIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new Currency(string.Empty));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCodeIsWhitespace()
    {
        Assert.Throws<ArgumentException>(() => new Currency("   "));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCodeIsNotThreeCharacters()
    {
        Assert.Throws<ArgumentException>(() => new Currency("US"));
        Assert.Throws<ArgumentException>(() => new Currency("USDA"));
    }

    [Fact]
    public void Constructor_ShouldSetCode_ToUpperInvariant()
    {
        var currency = new Currency("usd");
        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void TwoCurrencies_WithSameCode_ShouldBeEqual()
    {
        var currency1 = new Currency("USD");
        var currency2 = new Currency("USD");

        Assert.Equal(currency1, currency2);
        Assert.True(currency1 == currency2);
    }

    [Fact]
    public void Currency_ShouldBeCaseInsensitive()
    {
        var currency1 = new Currency("USD");
        var currency2 = new Currency("usd");

        Assert.Equal(currency1, currency2);
    }
}