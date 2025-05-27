using Moq;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.ExchangeRate.Providers;
using ExchangeRateUpdater.Models;

namespace ExchangeRate.Tests.ExchangeRate.Providers
{
    public class ExchangeRateServiceTests
    {
        private readonly Mock<ICzechApiClient> _apiClientMock;
        private readonly Mock<ILogger<ExchangeRateService>> _loggerMock;
        private readonly Mock<ICnbRatesCache> _dailyCacheMock;
        private readonly Mock<ICnbRatesCache> _monthlyCacheMock;
        private readonly ExchangeRateService _service;

        public ExchangeRateServiceTests()
        {
            _apiClientMock = new Mock<ICzechApiClient>();
            _loggerMock = new Mock<ILogger<ExchangeRateService>>();
            _dailyCacheMock = new Mock<ICnbRatesCache>();
            _monthlyCacheMock = new Mock<ICnbRatesCache>();

            _service = new ExchangeRateService(
                _apiClientMock.Object,
                _loggerMock.Object,
                _dailyCacheMock.Object,
                _monthlyCacheMock.Object
            );
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRates_FromDailyCache()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD"), new Currency("CZK") };
            var dailyRates = new Dictionary<string, decimal>
            {
                { "USD", 25.0m },
                { "CZK", 1.0m }
            };
            var monthlyRates = new Dictionary<string, decimal>();

            _dailyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(dailyRates);
            _monthlyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(monthlyRates);

            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
            Assert.Contains(result, r => r.SourceCurrency.Code == "USD" && r.Value == 25.0m);
            Assert.Contains(result, r => r.SourceCurrency.Code == "CZK" && r.Value == 1.0m);
        }


        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRates_FromMonthlyCache_IfNotInDaily()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("JPY") };
            var dailyRates = new Dictionary<string, decimal>();
            var monthlyRates = new Dictionary<string, decimal>
            {
                { "JPY", 0.2m }
            };

            _dailyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(dailyRates);
            _monthlyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(monthlyRates);

            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
            Assert.Single(result);
            Assert.Equal("JPY", result[0].SourceCurrency.Code);
            Assert.Equal(0.2m, result[0].Value);
        }

        [Fact]
        public async Task GetExchangeRateAsync_LogsWarning_IfCurrencyNotFound()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("ABC") };
            var dailyRates = new Dictionary<string, decimal>();
            var monthlyRates = new Dictionary<string, decimal>();

            _dailyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(dailyRates);
            _monthlyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(monthlyRates);

            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
            Assert.Empty(result);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Currency ABC not found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetExchangeRateAsync_From_OtherCountries_Monthly_IfNotFoundInDaily()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD") };
             var fakeJson = "{\"rates\":[{\"currencyCode\":\"USD\",\"rate\":25.0,\"amount\":1}]}";
            _apiClientMock.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeJson);

            var dailyRates = new Dictionary<string, decimal>();

            _dailyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(dailyRates);
            _monthlyCacheMock
                .Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .Returns<Func<Task<Dictionary<string, decimal>>>>(factory => factory());
            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
             Assert.Equal("USD", result[0].SourceCurrency.Code);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching monthly  exchange rates")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

         [Fact]
        public async Task When_Daily_And_Monthly_Returns_Nothing()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD") };
        
            var rates = new Dictionary<string, decimal>();

            _dailyCacheMock.Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(rates);
            _monthlyCacheMock
                .Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                 .ReturnsAsync(rates);
            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
            Assert.Empty(result);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Currency USD not found in CNB daily or monthly data")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsEmptyList_IfNoCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>();

            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDailyRatesAsync_ExecutesCallbackAndParsesRates()
        {
            // Arrange
            var fakeJson = "{\"rates\":[{\"currencyCode\":\"USD\",\"rate\":25.0,\"amount\":1}]}";
            _apiClientMock.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeJson);

            _dailyCacheMock
                .Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .Returns<Func<Task<Dictionary<string, decimal>>>>(factory => factory());

            _monthlyCacheMock
                .Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .Returns<Func<Task<Dictionary<string, decimal>>>>(factory => factory());

            var currencies = new List<Currency> { new Currency("USD") };

            // Act
            var result = await _service.GetExchangeRateAsync(currencies);

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].SourceCurrency.Code);
            Assert.Equal(25.0m, result[0].Value);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching daily exchange rates from Exchange rates API.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetExchangeRateAsync_WhenApiClientThrows_LogsErrorAndThrows()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD") };
            var exception = new Exception("Simulated API failure");

            _apiClientMock.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ThrowsAsync(exception);

            _dailyCacheMock
                .Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .Returns<Func<Task<Dictionary<string, decimal>>>>(factory => factory());

            _monthlyCacheMock
                .Setup(c => c.GetOrCreateAsync(It.IsAny<Func<Task<Dictionary<string, decimal>>>>()))
                .ReturnsAsync(new Dictionary<string, decimal>());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetExchangeRateAsync(currencies));
            Assert.Equal("Simulated API failure", ex.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to get exchange rates for requested currencies.")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}