using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Tests.Core;

public class CurrencyTests
{
    [Fact]
    public void Currency_ValidCode_ShouldCreate()
    {
        var currency = new Currency("USD");

        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Currency_LowercaseCode_ShouldConvertToUppercase()
    {
        var currency = new Currency("usd");

        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Currency_TooShortCode_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Currency("US"));
    }

    [Fact]
    public void Currency_TooLongCode_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Currency("USDD"));
    }
}
