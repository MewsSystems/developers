using ExchangeRates.Core.Models;
using ExchangeRates.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRates.Tests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public void GetExchangeRates_WhenCurrencyExists_ReturnsExpectedRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();

            var provider = new ExchangeRateProvider(httpClient, settings);

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
            Assert.Contains(rates, r => r.SourceCurrency.Code == "PHP" && r.Value == 40.991m / 100m);
        }

        [Fact]
        public void GetExchangeRates_WhenCurrencyDoesNotExists_ReturnsExpectedRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();

            var provider = new ExchangeRateProvider(httpClient, settings);

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

        [Fact]
        public void GetExchangeRates_WhenCurrencyParameterIsEmpty_ReturnsExpectedRates()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsSuccessfulResponse();
            var settings = MockHelper.GetMockConfigurationSettings();

            var provider = new ExchangeRateProvider(httpClient, settings);

            // Act
            var currencies = new List<Currency>(); // Empty list
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.NotNull(rates);
            Assert.Empty(rates);
        }

        [Fact]
        public void GetExchangeRates_HandlesHttpRequestException()
        {
            // Arrange
            var httpClient = MockHelper.GetMockClientThatReturnsErrorResponse();
            var settings = MockHelper.GetMockConfigurationSettings();

            var provider = new ExchangeRateProvider(httpClient, settings);

            // Act
            var rates = provider.GetExchangeRates(new List<Currency>());

            // Assert
            Assert.Empty(rates);
        }
    }
}