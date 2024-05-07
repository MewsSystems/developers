using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Interfaces;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests.Providers
{
    public class CZKBankExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsRatesFromCache_WhenAvailable()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockCacheService = new Mock<ICacheService>();
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var cacheKey = "CZKBankExchangeRates_" + DateTime.UtcNow.ToString("yyyyMMdd");
            var cachedApiResponse = new ApiResponse
            {
                Rates = new List<ApiExchangeRate>
                {
                    new ApiExchangeRate { CurrencyCode = "USD", Rate = 22m }
                }
            };
            var cachedData = JsonConvert.SerializeObject(cachedApiResponse);

            mockCacheService.Setup(m => m.GetCachedDataAsync(cacheKey)).ReturnsAsync(cachedData);

            var provider = new CZKBankExchangeRateProvider(mockHttpClientService.Object, mockCacheService.Object);

            // Act
            var rates = await provider.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            // Assert
            Assert.NotNull(rates);
            Assert.Single(rates);
            Assert.Equal(22m, rates.First().Value);
            mockHttpClientService.Verify(m => m.FetchDataAsync(It.IsAny<string>()), Times.Never);  // Ensure API was not called
        }

        [Fact]
        public async Task GetExchangeRatesAsync_FetchesFromApi_WhenCacheIsEmpty()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockCacheService = new Mock<ICacheService>();
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var currentDate = DateTime.UtcNow;
            var cacheKey = "CZKBankExchangeRates_" + currentDate.ToString("yyyyMMdd");
            var apiResponse = new ApiResponse
            {
                Rates = new List<ApiExchangeRate>
                {
                    new ApiExchangeRate { CurrencyCode = "USD", Rate = 22m }
                }
            };
            var responseData = JsonConvert.SerializeObject(apiResponse);

            mockCacheService.Setup(m => m.GetCachedDataAsync(cacheKey)).ReturnsAsync((string)null);
            mockHttpClientService.Setup(m => m.FetchDataAsync(It.IsAny<string>())).ReturnsAsync(responseData);

            var provider = new CZKBankExchangeRateProvider(mockHttpClientService.Object, mockCacheService.Object);

            // Act
            var rates = await provider.GetExchangeRatesAsync(currencies, currentDate);

            // Assert
            Assert.NotNull(rates);
            Assert.Single(rates);
            Assert.Equal(22m, rates.First().Value);
            mockCacheService.Verify(m => m.SaveDataToCacheAsync(cacheKey, responseData), Times.Once);  // Ensure data is cached
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenApiFails()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockCacheService = new Mock<ICacheService>();
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var currentDate = DateTime.UtcNow;
            var cacheKey = "CZKBankExchangeRates_" + currentDate.ToString("yyyyMMdd");

            mockCacheService.Setup(m => m.GetCachedDataAsync(cacheKey)).ReturnsAsync((string)null);
            mockHttpClientService.Setup(m => m.FetchDataAsync(It.IsAny<string>())).ReturnsAsync((string)null);

            var provider = new CZKBankExchangeRateProvider(mockHttpClientService.Object, mockCacheService.Object);

            // Act
            var rates = await provider.GetExchangeRatesAsync(currencies, currentDate);

            // Assert
            Assert.Empty(rates);
        }
    }
}
