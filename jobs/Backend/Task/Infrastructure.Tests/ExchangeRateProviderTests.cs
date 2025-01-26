using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Configurations;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace Infrastructure.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<IHttpClientService> _mockHttpClientService;
        private readonly Mock<IOptions<CNBConfig>> _mockConfig;
        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderTests()
        {
            _mockCacheService = new Mock<ICacheService>();
            _mockHttpClientService = new Mock<IHttpClientService>();
            _mockConfig = new Mock<IOptions<CNBConfig>>();

            // Set up the configuration mock
            _mockConfig.Setup(x => x.Value).Returns(new CNBConfig
            {
                BaseURL = "https://google.com",
                ExchangeRateURL = "",
                RefreshTimeHour = 23,
                RefreshTimeMinute = 30
            });

            // Instantiate the ExchangeRateProvider
            _provider = new ExchangeRateProvider(
                _mockCacheService.Object,
                _mockHttpClientService.Object,
                _mockConfig.Object
            );
        }

        [Fact]
        public async Task GetDailyExchangeRates_ShouldReturn()
        {
            // Arrange
            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate(
                    new Currency("USD"),
                    new Currency("EUR"),
                    0.85m
                )
            };

            _mockCacheService.Setup(x => x.Get<List<ExchangeRate>>(It.IsAny<string>())).Returns(exchangeRates);

            // Act
            var result = await _provider.GetDailyExchangeRates(new Currency("CZK"));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ShouldCallHttpClient()
        {
            var exchangeRates = new RawExchangeRates
            {
                Rates = new List<RawExchangeRate>
                {
                    new RawExchangeRate
                    {
                        Country = "UK",
                        Currency = "Pounds",
                        CurrencyCode = "GBP",
                        Rate = 18.77m
                    }
                }
            };

            _mockHttpClientService.Setup(x => x.GetJsonAsync<RawExchangeRates>(It.IsAny<string>()))
                .ReturnsAsync(exchangeRates);

            // Act
            var result = await _provider.GetDailyExchangeRates(new Currency("CZK"));

            // Assert
            Assert.NotNull(result);
            _mockHttpClientService.Verify(x => x.GetJsonAsync<RawExchangeRates>(It.IsAny<string>()), Times.Once); // Ensure HTTP request was made
        }

        [Fact]
        public async Task GetExchangeRate_ShouldReturnCorrectRate_WhenValidCurrenciesAreProvided()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate(
                    sourceCurrency,
                    targetCurrency,
                    (decimal)0.85
                )
            };

            _mockCacheService.Setup(x => x.Get<List<ExchangeRate>>(It.IsAny<string>())).Returns(exchangeRates);

            // Act
            var result = await _provider.GetExchangeRate(sourceCurrency, targetCurrency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((decimal)0.85, result?.Value); // Check if the correct exchange rate was returned
        }

        [Fact]
        public async Task GetExchangeRate_ShouldReturnNull_WhenRateNotFound()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("GBP");
            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate(
                    sourceCurrency,
                    targetCurrency,
                    (decimal)0.85
                )
            };

            _mockCacheService.Setup(x => x.Get<List<ExchangeRate>>(It.IsAny<string>())).Returns(exchangeRates);

            // Act
            var result = await _provider.GetExchangeRate(sourceCurrency, new Currency("EUR"));

            // Assert
            Assert.Null(result); // Ensure no exchange rate was found for the requested currencies
        }
    }
}