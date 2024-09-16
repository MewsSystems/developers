using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using Moq;

namespace ExchangeRateFinder.Infrastructure.UnitTests.Services
{
    public class CachingServiceTests
    {
        private readonly CachingService<ExchangeRate> _target;

        public CachingServiceTests() 
        {
            _target = new CachingService<ExchangeRate>();
        }

        [Fact]
        public async Task GetOrAddAsync_ReturnsCachedItem_WhenKeyExists()
        {
            // Arrange
            var key = "USD";
            var cachedItem = new ExchangeRate()
            {
                CountryName = "USA",
                SourceCurrencyCode = "CZK",
                TargetCurrencyCode = "USD",
                SourceCurrencyName = "dollar",
                Amount = 1,
                Value = 2.5m,
            };


            var mockGetItemCallback = new Mock<Func<Task<ExchangeRate>>>();
            mockGetItemCallback.Setup(callback => callback()).ReturnsAsync(cachedItem);


            // Act
            _target.UpdateCache(new Dictionary<string, ExchangeRate> { { key, cachedItem } });
            var result = await _target.GetOrAddAsync(key, mockGetItemCallback.Object);

            // Assert
            Assert.Equal(cachedItem.CountryName, result.CountryName);
            Assert.Equal(cachedItem.Value, result.Value);
            Assert.Equal(cachedItem.Amount, result.Amount);
            mockGetItemCallback.Verify(callback => callback(), Times.Never);
        }

        [Fact]
        public async Task GetOrAddAsync_CallsCallback_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = "USD";
            var exchangeRate = new ExchangeRate()
            {
                CountryName = "USA",
                SourceCurrencyCode = "CZK",
                TargetCurrencyCode = "USD",
                SourceCurrencyName = "dollar",
                Amount = 1,
                Value = 2.5m,
            };

            var mockGetItemCallback = new Mock<Func<Task<ExchangeRate>>>();
            mockGetItemCallback.Setup(callback => callback()).ReturnsAsync(exchangeRate);

            // Act
            var result = await _target.GetOrAddAsync(key, mockGetItemCallback.Object);

            // Assert
            Assert.Equal(exchangeRate.CountryName, result.CountryName);
            Assert.Equal(exchangeRate.Value, result.Value);
            Assert.Equal(exchangeRate.Amount, result.Amount);
            mockGetItemCallback.Verify(callback => callback(), Times.Once);
        }

        [Fact]
        public void UpdateCache_AddsNewKeysToCache_WithNewKeys()
        {
            // Arrange
            var oldCache = new Dictionary<string, ExchangeRate>
            {
                { "key1", new ExchangeRate()},
                { "key2",  new ExchangeRate()}
            };


            var newCache = new Dictionary<string, ExchangeRate>
            {
                { "key1",  new ExchangeRate() },
                { "key2",  new ExchangeRate()},
                { "key3",  new ExchangeRate() }
            };

            // Act
            var oldCacheUpdated = _target.UpdateCache(oldCache);
            var newCacheUpdated = _target.UpdateCache(newCache);

            // Assert
            Assert.Equal(3, newCacheUpdated.Count);
            Assert.True(newCacheUpdated.ContainsKey("key3"));
        }

        [Fact]
        public void UpdateCache_UpdatesExistingKeysInCache_WithExistingKeys()
        {
            // Arrange
            var oldCache = new Dictionary<string, ExchangeRate>
            {
                { "key1", new ExchangeRate() { Amount = 2 } }
            };


            var newCache = new Dictionary<string, ExchangeRate>
            {
                { "key1",  new ExchangeRate() {Amount = 3} },
                { "key2",  new ExchangeRate() },
                { "key3",  new ExchangeRate() }
            };

            // Act
            var oldCacheUpdated = _target.UpdateCache(oldCache);
            var newCacheUpdated = _target.UpdateCache(newCache);


            // Assert
            Assert.Equal(3, newCacheUpdated.Count);
            Assert.True(newCacheUpdated.ContainsKey("key1"));
            Assert.Equal(3, newCacheUpdated["key1"].Amount);
        }

        [Fact]
        public void UpdateCache_WithRemovedKeys_RemovesKeysFromCache()
        {
            // Arrange
            var oldCache = new Dictionary<string, ExchangeRate>
            {
                { "key1", new ExchangeRate() { Amount = 2 } },
                { "key3", new ExchangeRate() { Amount = 3 } }
            };


            var newCache = new Dictionary<string, ExchangeRate>
            {
                { "key3",  new ExchangeRate() }
            };

            // Act
            var oldCacheUpdated = _target.UpdateCache(oldCache);
            var newCacheUpdated = _target.UpdateCache(newCache);

            // Assert
            Assert.Equal(1, newCacheUpdated.Count);
            Assert.False(newCacheUpdated.ContainsKey("key1"));
        }
    }
}