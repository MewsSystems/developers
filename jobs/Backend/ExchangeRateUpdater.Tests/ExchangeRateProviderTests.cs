namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task Test1()
    {
        var provider = new ExchangeRateProvider();
        var rates = await provider.GetExchangeRates(Program.currencies);
        Assert.True(rates.Count() > 0);
    }
}