using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Core.Entities;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateProviderTests
    {
        /// <summary>
        /// Valid case: CNB API returns rates for requested currencies (USD, JPY).
        /// Expected outcome: EUR is ignored, and rates are normalized.
        /// </summary>
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsExpectedRates_WhenResponseIsValid()
        {
            // Arrange
            var jsonResponse = @"{
                ""rates"": [
                    {
                        ""amount"": 1,
                        ""country"": ""United States"",
                        ""currency"": ""Dollar"",
                        ""currencyCode"": ""USD"",
                        ""rate"": 22.00,
                        ""validFor"": ""2023-03-01T00:00:00"",
                        ""order"": 94
                    },
                    {
                        ""amount"": 100,
                        ""country"": ""Japan"",
                        ""currency"": ""Yen"",
                        ""currencyCode"": ""JPY"",
                        ""rate"": 16.00,
                        ""validFor"": ""2023-03-01T00:00:00"",
                        ""order"": 101
                    }
                ]
            }";

            var (provider, _, handlerMock) = CnbTestHelper.CreateProviderAndDependencies(jsonResponse, HttpStatusCode.OK);
            var currencies = new List<Currency> { new("USD"), new("EUR"), new("JPY") };
            var date = new DateTime(2023, 3, 1);

            // Act
            var results = await provider.GetExchangeRatesAsync(currencies, date).ToListAsync();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Single(results, r => r.TargetCurrency.Code == "USD" && r.Rate == 22.00m);
            Assert.Single(results, r => r.TargetCurrency.Code == "JPY" && r.Rate == 0.16m);
            Assert.DoesNotContain(results, r => r.TargetCurrency.Code == "EUR");

            // Verify CNB API was called once with the expected URL.
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().StartsWith("https://api.cnb.cz/cnbapi/exrates/daily?date=")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Edge case: CNB API returns an empty rate list.
        /// Expected outcome: No exchange rates returned.
        /// </summary>
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenNoRatesInResponse()
        {
            // Arrange
            var jsonResponse = @"{ ""rates"": [] }";
            var (provider, _, _) = CnbTestHelper.CreateProviderAndDependencies(jsonResponse, HttpStatusCode.OK);
            var currencies = new List<Currency> { new("USD") };
            var date = new DateTime(2023, 3, 1);

            // Act
            var results = await provider.GetExchangeRatesAsync(currencies, date).ToListAsync();

            // Assert
            Assert.Empty(results);
        }

        /// <summary>
        /// Invalid case: CNB API returns malformed JSON.
        /// Expected outcome: The provider should throw a JsonException.
        /// </summary>
        [Fact]
        public async Task GetExchangeRatesAsync_ThrowsJsonException_WhenResponseIsInvalidJson()
        {
            // Arrange
            var invalidJson = @"{ ""invalid"": }"; // Malformed JSON.
            var (provider, _, _) = CnbTestHelper.CreateProviderAndDependencies(invalidJson, HttpStatusCode.OK);
            var currencies = new List<Currency> { new("USD") };
            var date = new DateTime(2023, 3, 1);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(async () =>
            {
                await provider.GetExchangeRatesAsync(currencies, date).ToListAsync();
            });
        }

        /// <summary>
        /// Error case: CNB API returns a server error (500).
        /// Expected outcome: An HttpRequestException is thrown.
        /// </summary>
        [Fact]
        public async Task GetExchangeRatesAsync_ThrowsException_WhenApiReturnsServerError()
        {
            // Arrange
            var (provider, _, _) = CnbTestHelper.CreateProviderAndDependencies("Server error", HttpStatusCode.InternalServerError);
            var currencies = new List<Currency> { new("USD") };
            var date = new DateTime(2023, 3, 1);

            // Act & Assert
            await Assert.ThrowsAnyAsync<HttpRequestException>(async () =>
            {
                await provider.GetExchangeRatesAsync(currencies, date).ToListAsync();
            });
        }

        /// <summary>
        /// Edge case: CNB API response is `null`.
        /// Expected outcome: A warning is logged, and an empty result is returned.
        /// </summary
        [Fact]
        public async Task GetExchangeRatesAsync_LogsWarningAndReturnsEmpty_WhenCnbResponseIsNull()
        {
            // Arrange: Use the JSON literal "null" to simulate a null response.
            var nullResponse = "null";
            var (provider, loggerMock, _) = CnbTestHelper.CreateProviderAndDependencies(nullResponse, HttpStatusCode.OK);
            var currencies = new List<Currency> { new("USD") };
            var date = new DateTime(2023, 3, 1);

            // Act
            var results = await provider.GetExchangeRatesAsync(currencies, date).ToListAsync();

            // Assert
            Assert.Empty(results);

            // Verify that a warning was logged
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("CNB response is null")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }
    }
}
