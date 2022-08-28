namespace ExchangeRateUpdater.Tests;

public class IExchangeRateProviderTests : TestBase
{
    [Fact]
    public async Task BasicTest()
    {
        var rates = await _exchangeRateProvider.GetExchangeRates();
        //result must not be null
        Assert.NotNull(rates);
        //result must be greater than zero
        Assert.True(rates.Count() > 0);
        //assume always that a EUR exchange rate exists and is between 1 & 100
        Assert.True(rates.Count(p => p.TargetCurrency.Code == "EUR" && p.Value > 1 && p.Value < 100) > 0);
    }

    //do schema check
}