using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using Moq;
using static ExchangeRateUpdater.Tests.Constants;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    readonly IExchangeRateSource _integrationSource;
    public ExchangeRateProviderTests()
    {
        var integrationSource = new Mock<IExchangeRateSource>();
        integrationSource.Setup(i => i.GetExchangeRates())
            .Returns(() => new List<ExchangeRate>() { EURCZK, EURJPY, USDEUR });

        _integrationSource = integrationSource.Object;
    }

    [Fact()]
    public void GetExchangeRatesSingleCurrencyTest()
    {
        var exchangeRateProvider = new ExchangeRateProvider(_integrationSource);
        var rates = exchangeRateProvider.GetExchangeRates(new List<Currency>() { CZK });
        Assert.Single(rates);
    }


    [Fact()]
    public void GetExchangeRatesSourceAndTargetTest()
    {
        var exchangeRateProvider = new ExchangeRateProvider(_integrationSource);
        var rates = exchangeRateProvider.GetExchangeRates(new List<Currency>() { EUR });
        Assert.Equal(3, rates.Count());
    }

    [Fact()]
    public void GetExchangeRatesSourceCurrencyTest()
    {
        var exchangeRateProvider = new ExchangeRateProvider(_integrationSource);
        var rates = exchangeRateProvider.GetExchangeRates(new List<Currency>() { USD });
        Assert.Single(rates);
    }

    [Fact()]
    public void GetExchangeRatesMultipleCurrencyTest()
    {
        var exchangeRateProvider = new ExchangeRateProvider(_integrationSource);
        var rates = exchangeRateProvider.GetExchangeRates(new List<Currency>() { EUR, CZK });
        Assert.Equal(3, rates.Count());
    }
}