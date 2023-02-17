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
        integrationSource.Setup(i => i.GetSourceExchangeRates(CZK))
            .Returns(() => Enumerable.Empty<ExchangeRate>());
        integrationSource.Setup(i => i.GetTargetExchangeRates(CZK))
            .Returns(() => new List<ExchangeRate>() { EURCZK });

        integrationSource.Setup(i => i.GetSourceExchangeRates(EUR))
            .Returns(() => new List<ExchangeRate>() { EURCZK, EURJPY });
        integrationSource.Setup(i => i.GetTargetExchangeRates(EUR))
            .Returns(() => new List<ExchangeRate>() { USDEUR });

        integrationSource.Setup(i => i.GetSourceExchangeRates(USD))
            .Returns(() => new List<ExchangeRate>() { USDEUR });
        integrationSource.Setup(i => i.GetTargetExchangeRates(USD))
            .Returns(() => Enumerable.Empty<ExchangeRate>());
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
    public void GetExchangeRatesSourceCurrencyTargetTest()
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
}