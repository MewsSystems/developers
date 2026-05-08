using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdaterTests.StartupTests;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.Tests.Integration
{
    public class ExchangeRateProviderIntegrationTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsCombinedRates_FromAllFetchers()
        {
            // Arrange

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("https://mock-daily")
                .Respond("text/plain", @"
                11 Jun 2025 #123
                Country|Currency|Amount|Code|Rate
                United States|Dollar|1|USD|22,123");

            mockHttp.When("https://mock-other")
                .Respond("text/plain", @"
                11 Jun 2025 #123
                Country|Currency|Amount|Code|Rate|Source
                Switzerland|Franc|1|CHF|25,456|SNB");

            var httpClient = mockHttp.ToHttpClient();

            var inMemorySettings = new Dictionary<string, string>
            {
                ["CzechBankSettings:DailyRatesUrl"] = "https://mock-daily",
                ["CzechBankSettings:OtherCurrencyRatesUrl"] = "https://mock-other",
                ["CzechBankSettings:TimeoutSeconds"] = "10",
                ["CzechBankSettings:RetryCount"] = "2"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();
            var startup = new StartupForTest(config);
            startup.ConfigureServices(services);

            services.AddSingleton(httpClient);

            var provider = services.BuildServiceProvider();

            var httpFactory = new TestHttpClientFactory(httpClient);
            services.AddSingleton<IHttpClientFactory>(httpFactory);

            var finalProvider = services.BuildServiceProvider();

            var exchangeRateProvider = finalProvider.GetRequiredService<IExchangeRateProviderService>();

            // Act
            var currencies = new[] { new Currency("USD"), new Currency("CHF") };
            var rates = await exchangeRateProvider.GetExchangeRateAsync(currencies);

            // Assert
            Assert.NotEmpty(rates);
            Assert.Contains(rates, r => r.TargetCurrency.Code == "USD");
            Assert.Contains(rates, r => r.TargetCurrency.Code == "CHF");
        }

        // IHttpClientFactory override
        private class TestHttpClientFactory : IHttpClientFactory
        {
            private readonly HttpClient _client;

            public TestHttpClientFactory(HttpClient client)
            {
                _client = client;
            }

            public HttpClient CreateClient(string name) => _client;
        }
    }
}
