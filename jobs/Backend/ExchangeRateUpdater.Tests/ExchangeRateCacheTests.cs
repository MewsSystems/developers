using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Reflection;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateCacheTests
    {
        private const string ValidDailyRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string ValidMonthlyRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";
        private const string InvalidUrl = "https://www.invalidurl.com/monthly.txt";


        /// <summary>
        /// Tests that the ExchangeRateCache constructor initializes the cache field.
        /// </summary>
        [Test]
        public void TestExchangeRateCacheConstructor()
        {
            // Arrange
            var mockMemoryCache = new Mock<IMemoryCache>();

            // Act
            var exchangeRateCache = new ExchangeRateCache(mockMemoryCache.Object);

            // Assert
            Assert.That(exchangeRateCache, Is.Not.Null);
        }


        /// <summary>
        /// Tests the GetDailyRatesAsync method with a valid dailyRatesUrl.
        /// </summary>
        [Test]
        public async Task GetDailyRatesAsync_ReturnsRates()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var exchangeRateCache = new ExchangeRateCache(cache);

            // Act
            var rates = await exchangeRateCache.GetDailyRatesAsync(ValidDailyRatesUrl);

            // Assert
            Assert.That(rates, Is.Not.Null);
            Assert.That(rates.Any(), Is.True);
        }

        /// <summary>
        /// Tests the GetDailyRatesAsync method with an invalid dailyRatesUrl.
        /// Expects the method to throw a HttpRequestException.
        /// </summary>
        [Test]
        public async Task GetDailyRatesAsync_ThrowsHttpRequestExceptionForInvalidUrl()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var exchangeRateCache = new ExchangeRateCache(cache);

            // Act and assert
            Assert.ThrowsAsync<HttpRequestException>(() => exchangeRateCache.GetDailyRatesAsync(InvalidUrl));
        }

        [Test]
        public async Task GetMonthlyRatesAsync_ReturnsCachedRates()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var exchangeRateCache = new ExchangeRateCache(cache);
            var rates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), Currency.CZK, 20m),
                new ExchangeRate(new Currency("EUR"), Currency.CZK, 25m),
                new ExchangeRate(new Currency("GBP"), Currency.CZK, 30m)
            };
            var monthlyRatesCacheKeyField = typeof(ExchangeRateCache).GetField("MonthlyRatesCacheKey", BindingFlags.NonPublic | BindingFlags.Static);
            var monthlyRatesCacheKey = monthlyRatesCacheKeyField.GetValue(exchangeRateCache);
            cache.Set(monthlyRatesCacheKey, rates);

            // Act
            var result = await exchangeRateCache.GetMonthlyRatesAsync(ValidMonthlyRatesUrl);

            // Assert
            Assert.That(result, Is.EqualTo(rates));
        }



    }
}
