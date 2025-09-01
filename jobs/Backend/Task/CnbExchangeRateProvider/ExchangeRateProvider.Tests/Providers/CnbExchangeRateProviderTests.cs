using System.Net;
using System.Net.Http;
using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExchangeRateProvider.Tests.Providers
{
    public class CnbExchangeRateProviderTests
    {
        private static HttpClient CreateMockHttpClient(string response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handler = new MockHttpMessageHandler(response, statusCode);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://api.cnb.cz");
            return client;
        }

        private static IHttpClientFactory CreateMockHttpClientFactory(string response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpClient = CreateMockHttpClient(response, statusCode);
            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(f => f.CreateClient("CnbExchangeRateProvider")).Returns(httpClient);
            return factoryMock.Object;
        }

        [Fact]
        public async Task Returns_Only_Existing_CNB_Rates()
        {
            // Arrange (JSON format)
            var json = @"{
                ""date"": ""2025-08-31"",
                ""rates"": [
                    {
                        ""currencyCode"": ""USD"",
                        ""amount"": 1,
                        ""rate"": 22.00,
                        ""currency"": ""dollar"",
                        ""country"": ""United States""
                    },
                    {
                        ""currencyCode"": ""EUR"",
                        ""amount"": 1,
                        ""rate"": 24.00,
                        ""currency"": ""euro"",
                        ""country"": ""EMU""
                    }
                ]
            }";
            var httpClientFactory = CreateMockHttpClientFactory(json);

            var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);
            var requested = new[] { new Currency("USD"), new Currency("EUR"), new Currency("FOO") };

            // Act
            var rates = await provider.GetExchangeRatesAsync(requested);

            // Assert
            Assert.Contains(rates, r => r.SourceCurrency.Code == "USD");
            Assert.Contains(rates, r => r.SourceCurrency.Code == "EUR");
            Assert.DoesNotContain(rates, r => r.SourceCurrency.Code == "FOO");
        }

        [Fact]
        public async Task Handles_Malformed_Json_Gracefully()
        {
            // Arrange: malformed JSON
            var malformed = "{ invalid json";
            var httpClientFactory = CreateMockHttpClientFactory(malformed);
            var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);
            var requested = new[] { new Currency("USD") };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => provider.GetExchangeRatesAsync(requested));
        }

        [Fact]
        public async Task Handles_Network_Error_Gracefully()
        {
            // Arrange
            var httpClientFactory = CreateMockHttpClientFactory("", HttpStatusCode.InternalServerError);
            var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);
            var requested = new[] { new Currency("USD") };

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => provider.GetExchangeRatesAsync(requested));
        }

        [Fact]
        public async Task Returns_Only_Available_Rates_From_CNB()
        {
            // Arrange: Only USD rate available in JSON
            var json = @"{
                ""date"": ""2025-08-31"",
                ""rates"": [
                    {
                        ""currencyCode"": ""USD"",
                        ""amount"": 1,
                        ""rate"": 22.00
                    }
                ]
            }";
            var httpClientFactory = CreateMockHttpClientFactory(json);
            var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);

            // Test different scenarios
            var testCases = new[]
            {
                (new[] { new Currency("EUR") }, 0), // EUR not available
                (new[] { new Currency("USD"), new Currency("CZK") }, 1), // USD available, CZK not as source
                (new[] { new Currency("CZK") }, 0) // CZK not available as source
            };

            foreach (var (requested, expectedCount) in testCases)
            {
                // Create a new provider for each test case to avoid HttpClient disposal issues
                var testHttpClientFactory = CreateMockHttpClientFactory(json);
                var testProvider = new CnbExchangeRateProvider(testHttpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);

                var rates = await testProvider.GetExchangeRatesAsync(requested);
                Assert.Equal(expectedCount, rates.Count);

                if (expectedCount > 0)
                {
                    Assert.All(rates, r => Assert.Equal("CZK", r.TargetCurrency.Code));
                    Assert.DoesNotContain(rates, r => r.SourceCurrency.Code == "CZK");
                }
            }
        }

        [Fact]
        public async Task Handles_Empty_Or_Missing_Rates()
        {
            var testCases = new[]
            {
                // Empty rates array
                (@"{""date"": ""2025-08-31"", ""rates"": []}", "Empty rates array"),
                // Missing rates property
                (@"{""date"": ""2025-08-31""}", "Missing rates property"),
                // Null rates
                (@"{""date"": ""2025-08-31"", ""rates"": null}", "Null rates")
            };

            foreach (var (json, description) in testCases)
            {
                var httpClientFactory = CreateMockHttpClientFactory(json);
                var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);
                var requested = new[] { new Currency("USD") };

                var rates = await provider.GetExchangeRatesAsync(requested);
                Assert.Empty(rates);
            }
        }

        [Fact]
        public async Task Handles_Invalid_Rate_Data()
        {
            // Arrange: Invalid amount and rate values
            var json = @"{
                ""date"": ""2025-08-31"",
                ""rates"": [
                    {
                        ""currencyCode"": ""USD"",
                        ""amount"": 0,
                        ""rate"": -1
                    },
                    {
                        ""currencyCode"": ""EUR"",
                        ""amount"": 1,
                        ""rate"": 24.00
                    }
                ]
            }";
            var httpClientFactory = CreateMockHttpClientFactory(json);
            var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);
            var requested = new[] { new Currency("USD"), new Currency("EUR") };

            // Act
            var rates = await provider.GetExchangeRatesAsync(requested);

            // Assert: Only valid EUR rate should be returned
            Assert.Single(rates);
            Assert.Equal("EUR", rates.First().SourceCurrency.Code);
        }

        [Fact]
        public async Task Handles_Null_CurrencyCode()
        {
            // Arrange: One rate with null currencyCode
            var json = @"{
                ""date"": ""2025-08-31"",
                ""rates"": [
                    {
                        ""currencyCode"": null,
                        ""amount"": 1,
                        ""rate"": 22.00
                    },
                    {
                        ""currencyCode"": ""EUR"",
                        ""amount"": 1,
                        ""rate"": 24.00
                    }
                ]
            }";
            var httpClientFactory = CreateMockHttpClientFactory(json);
            var provider = new CnbExchangeRateProvider(httpClientFactory, NullLogger<CnbExchangeRateProvider>.Instance);
            var requested = new[] { new Currency("EUR") };

            // Act
            var rates = await provider.GetExchangeRatesAsync(requested);

            // Assert: Only EUR should be returned, null currencyCode ignored
            Assert.Single(rates);
            Assert.Equal("EUR", rates.First().SourceCurrency.Code);
        }

        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly string _response;
            private readonly HttpStatusCode _statusCode;
            public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
            {
                _response = response;
                _statusCode = statusCode;
            }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (_statusCode != HttpStatusCode.OK)
                {
                    return Task.FromResult(new HttpResponseMessage(_statusCode));
                }
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_response)
                });
            }
        }
    }
}
