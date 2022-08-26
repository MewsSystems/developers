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
        //assume always that a EUR exchange rate exists and is between 1 & 100
        Assert.True(rates.Count(p => p.TargetCurrency.Code == "EUR" && p.Value > 1 && p.Value < 100) > 0);
    }
}