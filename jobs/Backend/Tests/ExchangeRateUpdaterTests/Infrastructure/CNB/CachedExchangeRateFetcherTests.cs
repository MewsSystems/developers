using System.Text.Json;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.CNB;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdaterTests.Infrastructure.CNB
{
    public class CachedExchangeRateFetcherTests
    {
        private const string cacheKey = "exchange-rates";
        private readonly IDistributedCache cache;
        private readonly Mock<IExchangeRateFetcher> mockFetcher = new();
        private readonly CachedExchangeRateFetcher sut;

        public CachedExchangeRateFetcherTests()
        {
            var opts = Options.Create(new MemoryDistributedCacheOptions());
            cache = new MemoryDistributedCache(opts);
            sut = new CachedExchangeRateFetcher(cache, mockFetcher.Object);
            mockFetcher.Reset();
        }

        [Fact]
        public async Task GivenCacheHit_WhenGetExchangeRates_ThenReturnsCachedRates()
        {
            // Arrange
            var expectedRates = new List<ExchangeRate>
            {
                new(new Currency("CZK"), new Currency("USD"), 0.045m),
                new(new Currency("CZK"), new Currency("EUR"), 0.04m)
            };
            var serialized = JsonSerializer.Serialize(expectedRates);
            cache.SetString(cacheKey, serialized);

            // Act
            var result = await sut.GetExchangeRates();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedRates);
            mockFetcher.Verify(f => f.GetExchangeRates(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GivenCacheMiss_WhenGetExchangeRates_ThenFetchesAndCachesRates()
        {
            // Arrange
            var expectedRates = new List<ExchangeRate>
            {
                new(new Currency("CZK"), new Currency("USD"), 0.045m),
                new(new Currency("CZK"), new Currency("EUR"), 0.04m)
            };

            cache.Remove(cacheKey);

            mockFetcher.Setup(f => f.GetExchangeRates(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRates);

            // Act
            var result = await sut.GetExchangeRates();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedRates);
            mockFetcher.Verify(f => f.GetExchangeRates(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GivenFetcherThrows_WhenGetExchangeRates_ThenThrowsException()
        {
            // Arrange
            cache.Remove(cacheKey);

            mockFetcher.Setup(f => f.GetExchangeRates(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Fetcher error"));

            // Act
            var act = async () => await sut.GetExchangeRates();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Fetcher error");
        }
    }
}
