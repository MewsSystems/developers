using ExchangeRates.Core.Models;
using ExchangeRates.Core.Services;
using ExchangeRates.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRates.Tests
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
                new Currency("BRL"),
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
        /// Asserts that if there are no currencies requested by the currency parameter
        /// an empty list is succesfully returned.
        /// </summary>
        [Fact]
        public void GetExchangeRates_CurrencyParameterIsEmpty_ReturnsEmpty()
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
            Assert.Empty(rates);
        }

        /// <summary>
        /// Asserts that an empty list is returned and an error is logged
        /// if an exception is thrown during the request to fetch exchange rates.
        /// </summary>
        [Fact]
        public void GetExchangeRates_HttpRequestException_ReturnsEmptyAndLogsError()
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
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
            Assert.Empty(rates);
        }
    }
}