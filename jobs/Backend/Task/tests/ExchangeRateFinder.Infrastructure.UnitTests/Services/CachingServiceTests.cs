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
                TargetCurrencyName = "dollar",
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
                TargetCurrencyName = "dollar",
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
    }
}