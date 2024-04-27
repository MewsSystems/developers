using ExchangeRateFinder.Domain.Services;
using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;

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
                Country = "USA",
                SourceCurrency = "CZK",
                TargetCurrency = "USD",
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 2.5m,
            };


            var mockGetItemCallback = new Mock<Func<Task<ExchangeRate>>>();
            mockGetItemCallback.Setup(callback => callback()).ReturnsAsync(cachedItem);


            // Act
            _target.UpdateCache(new Dictionary<string, ExchangeRate> { { key, cachedItem } });
            var result = await _target.GetOrAddAsync(key, mockGetItemCallback.Object);

            // Assert
            Assert.Equal(cachedItem.Country, result.Country);
            Assert.Equal(cachedItem.Rate, result.Rate);
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
                Country = "USA",
                SourceCurrency = "CZK",
                TargetCurrency = "USD",
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 2.5m,
            };

            var mockGetItemCallback = new Mock<Func<Task<ExchangeRate>>>();
            mockGetItemCallback.Setup(callback => callback()).ReturnsAsync(exchangeRate);

            // Act
            var result = await _target.GetOrAddAsync(key, mockGetItemCallback.Object);

            // Assert
            Assert.Equal(exchangeRate.Country, result.Country);
            Assert.Equal(exchangeRate.Rate, result.Rate);
            Assert.Equal(exchangeRate.Amount, result.Amount);
            mockGetItemCallback.Verify(callback => callback(), Times.Once);
        }
    }
}