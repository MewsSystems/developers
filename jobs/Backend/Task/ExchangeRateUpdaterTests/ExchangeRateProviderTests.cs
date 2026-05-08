using AutoFixture;
using AutoFixture.Xunit2;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateProviderTests
{
    [Theory, LocalData]
    public async Task ThrowArgumentExceptionIfCurrenciesParamIsNull(ExchangeRateProvider provider)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => provider.GetExchangeRates(null));
    }

    [Theory, LocalData]
    public async Task ReturnEmptyListIfNoCurrenciesWasRequested(ExchangeRateProvider provider)
    {
        var rates = await provider.GetExchangeRates(Enumerable.Empty<Currency>());

        Assert.Empty(rates);
    }

    [Theory, LocalData]
    public async Task ReturnRatesFromCacheIfCacheHasIt(ExchangeRateProvider provider, Mock<IMemoryCache> currencyCache, Mock<IExchangeRateLoader> loader)
    {
        var rate = new ExchangeRate(new Currency(IsoCurrencyCode.EUR), new Currency(IsoCurrencyCode.CZK), 24.335M);

        object? valueMock = rate;
        currencyCache.Setup(c => c.TryGetValue(rate.SourceCurrency.Code, out valueMock)).Returns(true);

        var result = (await provider.GetExchangeRates([rate.SourceCurrency])).ToArray();

        loader.Verify(l => l.GetExchangeRatesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTime>()), Times.Never);

        Assert.Single(result);
        Assert.Equal(rate, result[0]);
    }

    [Theory, LocalData]
    public async Task ReturnRatesByLoaderIfCacheIsEmpty(ExchangeRateProvider provider, Mock<IMemoryCache> currencyCache, Mock<IExchangeRateLoader> loader)
    {
        var rate = new ExchangeRate(new Currency(IsoCurrencyCode.JPY), new Currency(IsoCurrencyCode.CZK), 0.13745M);

        object? valueMock = rate;
        currencyCache.Setup(c => c.TryGetValue(rate.SourceCurrency.Code, out valueMock)).Returns(false);

        loader
            .Setup(l => l.GetExchangeRatesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTime>()))
            .ReturnsAsync([rate]);

        var result = (await provider.GetExchangeRates([rate.SourceCurrency])).ToArray();

        Assert.Single(result);
        Assert.Equal(rate, result[0]);
    }

    [Theory, LocalData]
    public async Task ReturnRatesFromBothCacheAndLoader(ExchangeRateProvider provider, Mock<IMemoryCache> currencyCache, Mock<IExchangeRateLoader> loader)
    {
        var cachedRate = new ExchangeRate(new Currency(IsoCurrencyCode.EUR), new Currency(IsoCurrencyCode.CZK), 24.335M);

        object? valueMock = cachedRate;
        currencyCache.Setup(c => c.TryGetValue(cachedRate.SourceCurrency.Code, out valueMock)).Returns(true);

        var rateToLoad = new ExchangeRate(new Currency(IsoCurrencyCode.JPY), new Currency(IsoCurrencyCode.CZK), 0.13745M);
        loader
            .Setup(l => l.GetExchangeRatesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTime>()))
            .ReturnsAsync([rateToLoad]);

        var result = (await provider.GetExchangeRates([cachedRate.SourceCurrency, rateToLoad.SourceCurrency])).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Contains(cachedRate, result);
        Assert.Contains(rateToLoad, result);
    }
    
    [Theory, LocalData]
    public async Task PutLoadedRatesToCache(ExchangeRateProvider provider, Mock<IMemoryCache> currencyCache, Mock<IExchangeRateLoader> loader)
    {
        object? valueMock = null;
        currencyCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out valueMock)).Returns(false);

        var loadedRates = new[] {
            new ExchangeRate(new Currency(IsoCurrencyCode.EUR), new Currency(IsoCurrencyCode.CZK), 24.335M),
            new ExchangeRate(new Currency(IsoCurrencyCode.JPY), new Currency(IsoCurrencyCode.CZK), 0.13745M)
        };

        loader
            .Setup(l => l.GetExchangeRatesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTime>()))
            .ReturnsAsync(loadedRates);

        var result = (await provider.GetExchangeRates(loadedRates.Select(r => r.SourceCurrency))).ToArray();

        currencyCache.Verify(cache => cache.CreateEntry(IsoCurrencyCode.EUR), Times.Once);
        currencyCache.Verify(cache => cache.CreateEntry(IsoCurrencyCode.JPY), Times.Once);
    }

    private class LocalDataAttribute : AutoDataAttribute
    {
        public LocalDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            var loggerMock = new Mock<ILogger<ExchangeRateProvider>>();

            var loaderMock = new Mock<IExchangeRateLoader>();
            loaderMock.Setup(l => l.RateRefreshSchedule).Returns(new Mock<IRateRefreshScheduler>().Object);

            var currencyCacheMock = new Mock<IMemoryCache>();
            currencyCacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(new Mock<ICacheEntry>().Object);

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            dateTimeServiceMock.Setup(s => s.GetNow()).Returns(DateTime.Now);

            var provider = new ExchangeRateProvider(loaderMock.Object, loggerMock.Object, currencyCacheMock.Object, dateTimeServiceMock.Object);

            fixture.Inject(loaderMock);
            fixture.Inject(loggerMock);
            fixture.Inject(currencyCacheMock);
            fixture.Inject(provider);

            return fixture;
        }
    }
}