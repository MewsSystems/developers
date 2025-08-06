using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Tests.Models;

public class CurrencyTests
{
    [Fact]
    public void Currency_WithValidData_ShouldCreateSuccessfully()
    {
        const string code = "USD";

        var currency = new Currency(code);

        Assert.Equal(code, currency.Code);
    }

    [Fact]
    public void Currency_WithEmptyCode_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Currency(""));
        Assert.Throws<ArgumentException>(() => new Currency("   "));
        Assert.Throws<ArgumentException>(() => new Currency(null!));
    }

    [Fact]
    public void Currency_WithInvalidLength_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Currency("US"));
        Assert.Throws<ArgumentException>(() => new Currency("USDD"));
    }

    [Fact]
    public void Currency_ShouldConvertToUpperCase()
    {
        var currency = new Currency("usd");

        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Currency_Equals_ShouldReturnTrue_WhenSameCode()
    {
        var currency1 = new Currency("USD");
        var currency2 = new Currency("USD");

        Assert.Equal(currency1, currency2);
        Assert.True(currency1.Equals(currency2));
    }

    [Fact]
    public void Currency_Equals_ShouldReturnFalse_WhenDifferentCode()
    {
        var currency1 = new Currency("USD");
        var currency2 = new Currency("EUR");

        Assert.NotEqual(currency1, currency2);
        Assert.False(currency1.Equals(currency2));
    }

    [Fact]
    public void Currency_GetHashCode_ShouldReturnSameValue_WhenSameCode()
    {
        var currency1 = new Currency("USD");
        var currency2 = new Currency("USD");

        Assert.Equal(currency1.GetHashCode(), currency2.GetHashCode());
    }

    [Fact]
    public void Currency_ToString_ShouldReturnCode()
    {
        var currency = new Currency("USD");

        var result = currency.ToString();

        Assert.Equal("USD", result);
    }
} 