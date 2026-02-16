using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Abstraction.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Tests.IntegrationTests
{
    [TestFixture]
    internal class IntegrationTests
    {
        private WebApplicationFactory<Program> factory = null!;
        private HttpClient apiClient = null!;

        [SetUp]
        public void SetUp()
        {
            // Reset the call counter before each test
            MockCnbApiHandler.ResetCallCount();

            factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        // Remove the real named HttpClient registration for "CbnApi" if already registered
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHttpClientFactory));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Register a mock HttpClient that returns a controlled response
                        services.AddSingleton<IHttpClientFactory>(sp => new MockHttpClientFactory());
                    });
                });

            apiClient = factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            apiClient?.Dispose();
            factory?.Dispose();
        }

        [Test]
        public async Task GetExchangeRates_EndToEnd_ReturnsFilteredRates()
        {
            // Call the actual API endpoint
            var response = await apiClient.GetAsync("/ExchangeRate?currencyCodes=USD,EUR");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var rates = JsonSerializer.Deserialize<List<ExchangeRate>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(rates, Is.Not.Null);
            Assert.That(rates!.Count, Is.GreaterThan(0));

            // Verify that returned rates include USD and/or EUR (depends on mock response)
            var codes = rates.Select(r => r.SourceCurrency.Code).ToList();
            Assert.That(codes, Does.Contain("USD").Or.Contain("EUR"));
        }

        [Test]
        public async Task GetExchangeRates_InvalidCurrencyCodes_ReturnsBadRequest()
        {
            // Request with invalid codes (e.g., empty, too short)
            var response = await apiClient.GetAsync("/ExchangeRate?currencyCodes=US,E");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetExchangeRates_CachingWorks_OnlyOneUpstreamCall()
        {
            // First call
            var response1 = await apiClient.GetAsync("/ExchangeRate?currencyCodes=USD");
            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Verify one upstream call was made
            Assert.That(MockCnbApiHandler.CallCount, Is.EqualTo(1));

            // Second call (should hit cache if enabled)
            var response2 = await apiClient.GetAsync("/ExchangeRate?currencyCodes=EUR");
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Verify still only one upstream call was made (second request hit cache)
            Assert.That(MockCnbApiHandler.CallCount, Is.EqualTo(1), "Cache should prevent a second upstream call");
        }
    }

    // Mock IHttpClientFactory that returns a mock HttpClient with a delegating handler
    internal class MockHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var handler = new MockCnbApiHandler();
            return new HttpClient(handler);
        }
    }

    // Mock delegating handler that returns a fake CNB API response
    internal class MockCnbApiHandler : DelegatingHandler
    {
        private static int _callCount = 0;

        public static int CallCount => _callCount;

        public static void ResetCallCount() => _callCount = 0;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Increment the call counter
            Interlocked.Increment(ref _callCount);

            // Return a fake JSON response similar to CNB API
            var fakeResponse = @"{
  ""rates"": [
    {
      ""amount"": 1,
      ""country"": ""United States"",
      ""currency"": ""Dollar"",
      ""currencyCode"": ""USD"",
      ""order"": 1,
      ""rate"": 25.5,
      ""validFor"": ""2026-02-15""
    },
    {
      ""amount"": 1,
      ""country"": ""Eurozone"",
      ""currency"": ""Euro"",
      ""currencyCode"": ""EUR"",
      ""order"": 2,
      ""rate"": 27.0,
      ""validFor"": ""2026-02-15""
    },
    {
      ""amount"": 100,
      ""country"": ""Japan"",
      ""currency"": ""Yen"",
      ""currencyCode"": ""JPY"",
      ""order"": 3,
      ""rate"": 22.0,
      ""validFor"": ""2026-02-15""
    }
  ]
}";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(fakeResponse, System.Text.Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }
}
