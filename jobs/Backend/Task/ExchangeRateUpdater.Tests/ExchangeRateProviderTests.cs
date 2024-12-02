using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        /// <summary>
        /// Asserts that the exchange rate provider returns the expected exchange rates
        /// </summary>
        [Fact]
        public void GetExchangeRates_CurrencyExists_ReturnsExpectedRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();
            var logger = Mock.Of<ILogger<CnbExchangeRateProvider>>();
            var memoryCache = MockHelper.GetMemoryCacheMock();

            var provider = new CnbExchangeRateProvider(httpClient, settings, logger, memoryCache);

            // Act
            var currencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("brl"), // This currency exists, but is lowercase
                new Currency("PHP"),
            };
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.NotNull(rates);
            Assert.Equal(3, rates.Count());
            Assert.Contains(rates, r => r.SourceCurrency.Code == "AUD" && r.Value == 15.58m);
            Assert.Contains(rates, r => r.SourceCurrency.Code == "BRL" && r.Value == 3.983m);
            Assert.Contains(rates, r => r.SourceCurrency.Code == "PHP" && r.Value == 40.991m / 100);
        }

        /// <summary>
        /// Asserts a currency is ignored and not returned
        /// if it does not exist in the response from the exchange rate API.
        /// </summary>
        [Fact]
        public void GetExchangeRates_CurrencyDoesNotExists_IgnoreAndReturnsOtherRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();
            var logger = Mock.Of<ILogger<CnbExchangeRateProvider>>();
            var memoryCache = MockHelper.GetMemoryCacheMock();

            var provider = new CnbExchangeRateProvider(httpClient, settings, logger, memoryCache);

            // Act
            var currencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("BRL"),
                new Currency("XYZ") // This currency does not exist
            };
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.NotNull(rates);
            Assert.Equal(2, rates.Count());
            Assert.Contains(rates, r => r.SourceCurrency.Code == "AUD" && r.Value == 15.58m);
            Assert.Contains(rates, r => r.SourceCurrency.Code == "BRL" && r.Value == 3.983m);
        }

        /// <summary>
        /// Asserts that if the base currency is requested, all available exchange rates are returned.
        /// </summary>
        [Fact]
        public void GetExchangeRates_BaseCurrency_ReturnsAllAvailableRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();
            var logger = Mock.Of<ILogger<CnbExchangeRateProvider>>();
            var memoryCache = MockHelper.GetMemoryCacheMock();

            var provider = new CnbExchangeRateProvider(httpClient, settings, logger, memoryCache);

            var currencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("CZK"), // Base currency
            };
            // Act
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.NotNull(rates);
            Assert.Equal(31, rates.Count()); // All available rates
            Assert.Contains(rates, r => r.SourceCurrency.Code == "AUD" && r.Value == 15.58m);
        }

        /// <summary>
        /// Asserts that if there are no currencies requested by the currency parameter all available exchange rates are retured.
        /// </summary>
        [Fact]
        public void GetExchangeRates_CurrencyParameterIsEmpty_ReturnsAllRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();
            var logger = Mock.Of<ILogger<CnbExchangeRateProvider>>();
            var memoryCache = MockHelper.GetMemoryCacheMock();

            var provider = new CnbExchangeRateProvider(httpClient, settings, logger, memoryCache);

            // Act
            var currencies = new List<Currency>(); // Empty list
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.NotNull(rates);
            Assert.Equal(31, rates.Count());
        }

        /// <summary>
        /// Asserts that an exception is thrown and an error is logged
        /// if an exception is thrown during the request to fetch exchange rates.
        /// </summary>
        [Fact]
        public void GetExchangeRates_HttpRequestException_LogsErrorAndThrowsExceptions()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsErrorResponse();
            var settings = MockHelper.GetMockConfigurationSettings();
            var loggerMock = new Mock<ILogger<CnbExchangeRateProvider>>();
            var memoryCache = MockHelper.GetMemoryCacheMock();

            var provider = new CnbExchangeRateProvider(httpClient, settings, loggerMock.Object, memoryCache);

            // Act
            var currencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("BRL"),
                new Currency("PHP"),
            };

            Assert.Throws<HttpRequestException>(() => provider.GetExchangeRates(currencies));

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}