using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Providers.CNB;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Test;

public class ExchangeRateProviderTest
{
    [Fact]
    public async Task GetExchangeRatesAsync_UsesCache_WhenCalledTwice()
    {
        // Arrange
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var expectedCacheKey = $"{nameof(CNBCurrencyExchangeProvider)}_{targetDate:yyyy-MM-dd}";

        var options = Options.Create(new CNBExchangeRateProviderOptions
        {
            Name = "CNB",
            BaseUrl = "https://api.cnb.cz",
            CacheAbsoluteExpiration = TimeSpan.FromMinutes(10),
            CacheSlidingExpiration = TimeSpan.FromMinutes(2)
        });

        var apiRates = new List<ExchangeRate>
        {
            new(new Currency("EUR"), new Currency("CZK"), 25.10m),
            new(new Currency("USD"), new Currency("CZK"), 23.40m)
        };

        var clientMock = new Mock<ICNBExchangeRateApiClient>(MockBehavior.Strict);
        clientMock
            .Setup(x => x.FetchExchangeRatesAsync(
                It.Is<DateOnly>(d => d == targetDate),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiRates);

        var cacheMock = new Mock<IMemoryCache>(MockBehavior.Strict);

        var callCount = 0;

        object? cachedObject = null;

        cacheMock
            .Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedObject))
            .Returns((object key, out object? value) =>
            {
                Assert.Equal(expectedCacheKey, key);

                callCount++;

                if (callCount == 1)
                {
                    value = null;
                    return false;
                }

                value = apiRates;
                return true;
            });

        // Because _cache.Set(...) is an extension method, it calls CreateEntry internally.
        var entryMock = new Mock<ICacheEntry>();

        cacheMock
            .Setup(c => c.CreateEntry(It.Is<object>(k => k.Equals(expectedCacheKey))))
            .Returns(entryMock.Object);

        // The Set extension will set Value and Expiration properties and then dispose the entry.
        entryMock.SetupAllProperties();
        entryMock.Setup(e => e.Dispose());

        var sut = new CNBCurrencyExchangeProvider(clientMock.Object, cacheMock.Object, options);

        var requestedCurrencies = new[] { new Currency("EUR") };

        // Act

        // cache miss
        var first = (await sut.GetExchangeRatesAsync(requestedCurrencies)).ToList();

        // cache hit
        var second = (await sut.GetExchangeRatesAsync(requestedCurrencies)).ToList();

        Assert.Single(first);
        Assert.Single(second);
        Assert.Equal("EUR", first[0].SourceCurrency.Code);
        Assert.Equal("EUR", second[0].SourceCurrency.Code);

        // Assert

        // client called only once
        clientMock.Verify(x => x.FetchExchangeRatesAsync(
                It.Is<DateOnly>(d => d == targetDate),
                It.IsAny<CancellationToken>()),
            Times.Once);

        // cache was written only once
        cacheMock.Verify(c => c.CreateEntry(It.Is<object>(k => k.Equals(expectedCacheKey))), Times.Once);
        entryMock.VerifySet(e => e.Value = It.IsAny<object>(), Times.Once);
        entryMock.Verify(e => e.Dispose(), Times.Once);

        // TryGetValue called twice
        cacheMock.Verify(c => c.TryGetValue(It.IsAny<object>(), out cachedObject), Times.Exactly(2));
    }
}
