using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExchangeRateProvider.Tests.Providers
{
    /// <summary>
    /// Tests for distributed caching provider - focuses on cache hit/miss logic
    /// </summary>
    public class DistributedCachingExchangeRateProviderTests
    {
        [Fact]
        public async Task ReturnsCachedRates_OnCacheHit()
        {
            var mockInnerProvider = new Mock<IExchangeRateProvider>();
            var mockCache = new Mock<IDistributedCache>();
            var mockCacheStrategy = new Mock<CnbCacheStrategy>();

            var cachedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.0m)
            };

            // Setup cache hit
            mockCache.Setup(c => c.GetAsync("cnb_all_rates", It.IsAny<CancellationToken>()))
                .ReturnsAsync(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(cachedRates));

            // Ensure inner provider returns empty collection if called (shouldn't be called)
            mockInnerProvider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>());

            var provider = new DistributedCachingExchangeRateProvider(
                mockInnerProvider.Object, mockCache.Object, mockCacheStrategy.Object,
                NullLogger<DistributedCachingExchangeRateProvider>.Instance);

            // Act
            var result = await provider.GetExchangeRatesAsync([new Currency("USD")]);

            // Assert - Should return cached data without calling inner provider
            Assert.Single(result);
            Assert.Equal("USD", result.First().SourceCurrency.Code);
            mockInnerProvider.Verify(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task FetchesFreshRates_OnCacheMiss()
        {
            var mockInnerProvider = new Mock<IExchangeRateProvider>();
            var mockCache = new Mock<IDistributedCache>();
            var mockCacheStrategy = new Mock<CnbCacheStrategy>();

            var freshRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m)
            };

            // Setup cache miss
            mockCache.Setup(c => c.GetAsync("cnb_all_rates", It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null);

            mockInnerProvider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(freshRates);

            var provider = new DistributedCachingExchangeRateProvider(
                mockInnerProvider.Object, mockCache.Object, mockCacheStrategy.Object,
                NullLogger<DistributedCachingExchangeRateProvider>.Instance);

            // Act
            var result = await provider.GetExchangeRatesAsync([new Currency("EUR")]);

            // Assert - Should fetch from inner provider and cache result
            Assert.Single(result);
            Assert.Equal("EUR", result.First().SourceCurrency.Code);
            mockInnerProvider.Verify(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockCache.Verify(c => c.SetAsync("cnb_all_rates", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandlesPartialCacheHit_WithMissingCurrencies()
        {
            var mockInnerProvider = new Mock<IExchangeRateProvider>();
            var mockCache = new Mock<IDistributedCache>();
            var mockCacheStrategy = new Mock<CnbCacheStrategy>();

            var cachedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.0m)
            };

            var additionalRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m)
            };

            // Setup partial cache hit - USD cached, EUR missing
            mockCache.Setup(c => c.GetAsync("cnb_all_rates", It.IsAny<CancellationToken>()))
                .ReturnsAsync(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(cachedRates));

            // Mock should return additional rates when called with EUR
            mockInnerProvider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(additionalRates);

            var provider = new DistributedCachingExchangeRateProvider(
                mockInnerProvider.Object, mockCache.Object, mockCacheStrategy.Object,
                NullLogger<DistributedCachingExchangeRateProvider>.Instance);

            // Act - Request both cached and missing currencies
            var result = await provider.GetExchangeRatesAsync([new Currency("USD"), new Currency("EUR")]);

            // Assert - Should combine cached and fresh data
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.SourceCurrency.Code == "USD");
            Assert.Contains(result, r => r.SourceCurrency.Code == "EUR");
        }
    }
}
