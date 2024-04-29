using ExchangeRateUpdater;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateProviderTests 
{
    [Fact]
    public void GivenAnAUDCurrency_WhenGetExchangeRatesCalled_ResponseIsNotEmpty()
    {
        var provider = new ExchangeRateProvider();

        var rates = provider.GetExchangeRates(new []{ new Currency("AUD") });

        Assert.NotEmpty(rates);
    }
}


