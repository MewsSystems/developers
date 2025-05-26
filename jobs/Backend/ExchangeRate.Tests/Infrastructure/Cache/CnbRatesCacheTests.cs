using ExchangeRateUpdater.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRate.Tests.Infrastructure.Cache
{
    public class CnbRatesCacheTests
    {
        [Fact]
        public async Task GetOrCreateAsync_ReturnsValueFromFactory_WhenNotCached()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheKey = "test-key";
            var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
            var loggerMock = new Mock<ILogger<CnbRatesCache>>();
            var cache = new CnbRatesCache(memoryCache, cacheKey, () => expiration, loggerMock.Object);

            var expected = new Dictionary<string, decimal> { { "USD", 25.0m } };

            // Act
            var result = await cache.GetOrCreateAsync(() => Task.FromResult(expected));

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetOrCreateAsync_ReturnsCachedValue_WhenAlreadyCached()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheKey = "test-key";
            var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
            var loggerMock = new Mock<ILogger<CnbRatesCache>>();
            var cache = new CnbRatesCache(memoryCache, cacheKey, () => expiration, loggerMock.Object);

            var expected = new Dictionary<string, decimal> { { "EUR", 27.0m } };

            // Prime the cache
            await cache.GetOrCreateAsync(() => Task.FromResult(expected));

            // Act
            var result = await cache.GetOrCreateAsync(() => throw new Exception("Factory should not be called"));

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetOrCreateAsync_SetsExpirationCorrectly()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheKey = "test-key";
            var expiration = DateTimeOffset.UtcNow.AddSeconds(1);
            var loggerMock = new Mock<ILogger<CnbRatesCache>>();
            var cache = new CnbRatesCache(memoryCache, cacheKey, () => expiration, loggerMock.Object);

            var expected = new Dictionary<string, decimal> { { "CZK", 1.0m } };

            // Act
            var result = await cache.GetOrCreateAsync(() => Task.FromResult(expected));

            // Wait for expiration
            await Task.Delay(1100);

            // Verify that "Cache miss" was logged at least once (after expiration)
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Cache miss")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeast(1));
        }

        [Fact]
        public async Task GetOrCreateAsync_SetCache_ThrowsException()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheKey = "test-key";
            var expiration = DateTimeOffset.UtcNow.AddSeconds(1);
            var loggerMock = new Mock<ILogger<CnbRatesCache>>();
            var cache = new CnbRatesCache(memoryCache, cacheKey, () => expiration, loggerMock.Object);

            var exception = new Exception("Simulated exception");

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                 cache.GetOrCreateAsync(() => Task.FromException<Dictionary<string, decimal>>(exception))
            );  
            Assert.Equal("Simulated exception", ex.Message);

            // Verify that error was logged
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error occurred while fetching or caching data")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}