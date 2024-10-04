using ExchangeRateUpdater.ExchangeRateAPI.CBNClientApi;
using ExchangeRateUpdater.ExchangeRateAPI.DTOs;
using ExchangeRateUpdater.ExchangeRateAPI.ExchangeRateProvider;
using ExchangeRateUpdater.ExchangeRateAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateProjectTest
{
    public class ExchangeRateProviderTest
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        private void SetUpClient(Mock<ICBNClientApi> mockCBNClient, bool returnAUD = false)
        {
            var exchangeRateResponseMock = new ExchangeRatesResponseDTO
            {
                Rates = returnAUD ?
                new List<RateDTO>
                {
                    new RateDTO() { ValidFor = "2024-05-10", Order = 90, Country = "Austrálie", Currency = "dolar", Amount = 1, CurrencyCode = "AUD", Rate = 15.285M }
                } :
                new List<RateDTO>
                {
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "EMU", Currency = "euro", Amount = 1, CurrencyCode = "EUR", Rate = 24.935M },
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "Japonsko", Currency = "jen", Amount = 100, CurrencyCode = "JPY", Rate = 14.852M },
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "Brazílie", Currency = "real", Amount = 1, CurrencyCode = "BRL", Rate = 4.503M },
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "Indie", Currency = "rupie", Amount = 100, CurrencyCode = "INR", Rate = 27.698M },
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "Indonesie", Currency = "rupie", Amount = 1000, CurrencyCode = "IDR", Rate = 1.442M },
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "USA", Currency = "dolar", Amount = 1, CurrencyCode = "USD", Rate = 23.131M },
                    new RateDTO(){ ValidFor = "2024-05-10", Order = 90, Country = "Velká Británie", Currency = "libra", Amount = 1, CurrencyCode = "GBP", Rate = 28.979M }
                }
            };

            mockCBNClient.Setup(m => m.GetExratesDaily()).Returns(Task.FromResult(exchangeRateResponseMock));
        }

        private IConfigurationRoot SetUpConfiguraton()
        {
            var config = new Dictionary<string, string>
                {
                    {"CacheDurationInSeconds", "300"},
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            return configuration;
        }

        private Mock<IMemoryCache> SetUpCache()
        {
            var memoryCache = Mock.Of<IMemoryCache>();
            var cachEntry = Mock.Of<ICacheEntry>();

            var mockMemoryCache = Mock.Get(memoryCache);
            mockMemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            return mockMemoryCache;
        }

        [Fact]
        public void GivenAListOfCurrencyCodes_WhenGetExchangeRates_ThenCorrectExchangeRatesAreReturned()
        {
            // Arrange
            var mockCBNClient = new Mock<ICBNClientApi>();
            var mockLogger = new Mock<ILogger<CBNExchangeRateProvider>>();

            SetUpClient(mockCBNClient);
            var configuration = SetUpConfiguraton();
            var mockMemoryCache = SetUpCache();

            // Act
            var exchangeRateProvider = new CBNExchangeRateProvider(mockCBNClient.Object, mockMemoryCache.Object, configuration, mockLogger.Object);

            var result = exchangeRateProvider.GetExchangeRates(currencies);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Result.Count());
            Assert.Contains(result.Result, r => string.Equals(r.SourceCurrency.Code, "USD") || string.Equals(r.SourceCurrency.Code, "EUR") || string.Equals(r.SourceCurrency.Code, "JPY"));

            var jpyExchangeRate = result.Result.Where(x => string.Equals(x.SourceCurrency.Code, "JPY")).First();
            Assert.Equal(0.14852M, jpyExchangeRate.Value);
        }


        [Theory]
        [InlineData("usd")]
        [InlineData("USD")]
        public void GivenCurrencyCode_WhenGetExchangeRates_ThenCorrectExchangeRateIsReturned(string currency)
        {
            // Arrange
            var mockCBNClient = new Mock<ICBNClientApi>();
            var mockLogger = new Mock<ILogger<CBNExchangeRateProvider>>();

            SetUpClient(mockCBNClient);
            var configuration = SetUpConfiguraton();
            var mockMemoryCache = SetUpCache();

            // Act
            var exchangeRateProvider = new CBNExchangeRateProvider(mockCBNClient.Object, mockMemoryCache.Object, configuration, mockLogger.Object);

            var result = exchangeRateProvider.GetExchangeRates([new Currency(currency)]);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Result);
            Assert.Contains(result.Result, r => String.Equals(r.SourceCurrency.Code, "USD"));

            var usdExchangeRate = result.Result.Where(x => string.Equals(x.SourceCurrency.Code, "USD")).First();
            Assert.Equal(23.131M, usdExchangeRate.Value);
        }

        [Fact]
        public void GivenCurrencyCode_WhenGetExchangeRates_ThenNoRateIsFound()
        {
            // Arrange
            var mockCBNClient = new Mock<ICBNClientApi>();
            var mockLogger = new Mock<ILogger<CBNExchangeRateProvider>>();

            SetUpClient(mockCBNClient);
            var configuration = SetUpConfiguraton();
            var mockMemoryCache = SetUpCache();

            // Act
            var exchangeRateProvider = new CBNExchangeRateProvider(mockCBNClient.Object, mockMemoryCache.Object, configuration, mockLogger.Object);

            var result = exchangeRateProvider.GetExchangeRates([new Currency("HRK")]);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void GivenDifferentValuesInCacheAndFromProvider_WhenGetExchangeRates_ThenCachedValueIsUsed()
        {
            // Arrange
            var mockCBNClient = new Mock<ICBNClientApi>();
            var mockLogger = new Mock<ILogger<CBNExchangeRateProvider>>();

            var configuration = SetUpConfiguraton();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Act
            SetUpClient(mockCBNClient, true);

            var exchangeRateProvider = new CBNExchangeRateProvider(mockCBNClient.Object, memoryCache, configuration, mockLogger.Object);

            var result = exchangeRateProvider.GetExchangeRates([new Currency("AUD")]);

            SetUpClient(mockCBNClient);

            result = exchangeRateProvider.GetExchangeRates([new Currency("AUD")]);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Result);
            Assert.Contains(result.Result, r => String.Equals(r.SourceCurrency.Code, "AUD"));

            var usdExchangeRate = result.Result.Where(x => string.Equals(x.SourceCurrency.Code, "AUD")).First();
            Assert.Equal(15.285M, usdExchangeRate.Value);
        }
    }
}