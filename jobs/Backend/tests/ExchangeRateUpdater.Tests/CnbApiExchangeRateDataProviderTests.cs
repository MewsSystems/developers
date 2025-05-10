using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CnbApiExchangeRateDataProviderTests
    {
        private readonly CnbApiSettings _settings = new CnbApiSettings
        {
            BaseAddress = "https://api.cnb.cz/",
            Endpoint = "cnbapi/exrates/daily",
            BaseCurrency = "CZK"
        };

        private HttpClient CreateHttpClientWithResponse(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = statusCode,
                   Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
               })
               .Verifiable();

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(_settings.BaseAddress)
            };
            return client;
        }

        [Fact]
        public async Task GetNormalizedRatesAsync_ValidResponse_ReturnsNormalizedRates()
        {
            // Arrange: sample JSON response with USD.
            var jsonResponse = @"{
                ""rates"": [
                    {
                        ""amount"": 1,
                        ""currencyCode"": ""USD"",
                        ""rate"": 22.50
                    }
                ]
            }";

            var httpClient = CreateHttpClientWithResponse(jsonResponse);
            IExchangeRateDataProvider provider = new CnbApiExchangeRateDataProvider(httpClient, _settings);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("CZK")
            };

            // Act
            var normalizedRates = await provider.GetNormalizedRatesAsync(currencies);

            // Assert: USD should have normalized rate 22.50 and base currency CZK should be added with rate 1.
            Assert.NotNull(normalizedRates);
            Assert.Contains("USD", normalizedRates.Keys);
            Assert.Equal(22.50m, normalizedRates["USD"].Rate);
            // Also returns base currency, if requested, since it is not included in the response.
            Assert.Contains("CZK", normalizedRates.Keys);
            Assert.Equal(1m, normalizedRates["CZK"].Rate);
        }

        [Fact]
        public async Task GetNormalizedRatesAsync_EmptyApiResponse_ReturnsEmptyDictionary()
        {
            // Arrange: API returns empty rates array.
            var jsonResponse = @"{ ""rates"": [] }";
            var httpClient = CreateHttpClientWithResponse(jsonResponse);
            IExchangeRateDataProvider provider = new CnbApiExchangeRateDataProvider(httpClient, _settings);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("CZK")
            };

            // Act
            var normalizedRates = await provider.GetNormalizedRatesAsync(currencies);

            // Assert: No rates -> empty dictionary.
            Assert.NotNull(normalizedRates);
            Assert.Empty(normalizedRates);
        }

        [Fact]
        public async Task GetNormalizedRatesAsync_DuplicateCurrencies_FilteredCorrectly()
        {
            // Arrange: API response for USD only.
            var jsonResponse = @"{
                ""rates"": [
                    {
                        ""amount"": 1,
                        ""country"": ""USA"",
                        ""currency"": ""US Dollar"",
                        ""currencyCode"": ""USD"",
                        ""order"": 1,
                        ""rate"": 22.50,
                        ""validFor"": ""2025-05-09""
                    }
                ]
            }";

            var httpClient = CreateHttpClientWithResponse(jsonResponse);
            IExchangeRateDataProvider provider = new CnbApiExchangeRateDataProvider(httpClient, _settings);

            // Introduce duplicate currencies with different case variations.
            var currencies = new List<Currency>
            {
                new Currency("usd"),
                new Currency("USD"),
                new Currency("Usd")
            };

            // Act
            var normalizedRates = await provider.GetNormalizedRatesAsync(currencies);

            // Assert: Only one record for USD should be present.
            Assert.NotNull(normalizedRates);
            Assert.Single(normalizedRates);
            Assert.Contains("USD", normalizedRates.Keys);
            Assert.Equal(22.50m, normalizedRates["USD"].Rate);
        }

        [Fact]
        public async Task GetNormalizedRatesAsync_NullCurrencies_ReturnsEmptyDictionary()
        {
            // Arrange: null currencies.
            var jsonResponse = @"{ ""rates"": [] }";
            var httpClient = CreateHttpClientWithResponse(jsonResponse);
            IExchangeRateDataProvider provider = new CnbApiExchangeRateDataProvider(httpClient, _settings);

            // Act
            var normalizedRates = await provider.GetNormalizedRatesAsync(null);

            // Assert: returned dictionary is empty.
            Assert.NotNull(normalizedRates);
            Assert.Empty(normalizedRates);
        }

        [Fact]
        public async Task GetNormalizedRatesAsync_UnsupportedCurrency_ReturnsDictionaryWithoutThatCurrency()
        {
            // Arrange:
            // JSON response returns only for USD.
            var jsonResponse = @"{
                ""rates"": [
                    {
                        ""amount"": 1,
                        ""currencyCode"": ""USD"",
                        ""rate"": 22.50
                    }
                ]
            }";

            // Create an HttpClient with the given response.
            var httpClient = CreateHttpClientWithResponse(jsonResponse);

            // Prepare the provider with settings where BaseCurrency is "CZK".
            IExchangeRateDataProvider provider = new CnbApiExchangeRateDataProvider(httpClient, _settings);

            // Prepare a list that includes an unsupported currency "GBP" and the base currency "CZK".
            var currencies = new List<Currency>
            {
                new Currency("GBP"), // unsupported as it does not appear in the API response
                new Currency("CZK")  // base currency should be added with rate = 1
            };

            // Act
            var normalizedRates = await provider.GetNormalizedRatesAsync(currencies);

            // Assert:
            // The result should not contain "GBP"
            Assert.NotNull(normalizedRates);
            Assert.DoesNotContain("GBP", normalizedRates.Keys);
            // It should contain "CZK", as the base currency is always added if requested.
            Assert.Contains("CZK", normalizedRates.Keys);
            Assert.Equal(1m, normalizedRates["CZK"].Rate);
        }
    }
}
