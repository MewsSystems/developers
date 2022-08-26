namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task BasicTest()
    {
        var provider = new ExchangeRateProvider();
        var rates = await provider.GetExchangeRates(Program.currencies);
        Assert.NotNull(rates);
        Assert.True(rates.Count() > 0);
        //assume always a EUR exchange rate
        Assert.True(rates.Count(p => p.TargetCurrency.Code == "EUR") > 0);
    }
}