using ExchangeRateUpdater.Domain.Models;
using Xunit;

namespace ExchangeRateUpdater.Domain.UnitTests.Models;

public class CurrencyPairShould
{
    private readonly Currency _czkCurrency = Currency.FromString("CZK");
    private readonly Currency _usdCurrency = Currency.FromString("USD");

    [Fact]
    public void BeEqual()
    {
        // create using different strings (with same meaning)
        var currencyPair1 = new CurrencyPair(_usdCurrency, _czkCurrency);
        var currencyPair2 = new CurrencyPair(_usdCurrency, _czkCurrency);

        Assert.Equal(currencyPair1, currencyPair2);
    }

    [Fact]
    public void NotBeEqual()
    {
        var currencyPair1 = new CurrencyPair(_usdCurrency, _czkCurrency);
        var currencyPair2 = new CurrencyPair(_czkCurrency, _usdCurrency);

        Assert.NotEqual(currencyPair1, currencyPair2);
    }

    [Fact]
    public void NotEqualToNull()
    {
        var currencyPair1 = new CurrencyPair(_usdCurrency, _czkCurrency);
        var currencyPair2 = (CurrencyPair?)null;

        Assert.NotEqual(currencyPair1, currencyPair2);
    }
}
