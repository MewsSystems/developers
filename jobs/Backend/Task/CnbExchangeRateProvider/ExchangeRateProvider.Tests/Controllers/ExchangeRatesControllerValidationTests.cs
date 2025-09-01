using System.Net;
using ExchangeRateProvider.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateProvider.Tests.Controllers
{
    public class ExchangeRatesControllerValidationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ExchangeRatesControllerValidationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(_ => { });
        }

        [Fact]
        public async Task Returns_400_When_CurrencyCodes_Empty()
        {
            var client = _factory.CreateClient();
            var res = await client.GetAsync("/api/ExchangeRates?currencyCodes=");
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }

        [Fact]
        public async Task Returns_200_On_Health_And_Metrics()
        {
            var client = _factory.CreateClient();
            var health = await client.GetAsync("/health");
            Assert.Equal(HttpStatusCode.OK, health.StatusCode);

            var metrics = await client.GetAsync("/metrics");
            Assert.Equal(HttpStatusCode.OK, metrics.StatusCode);
        }

        [Fact]
        public async Task Returns_400_When_Too_Many_Currencies()
        {
            var cfgFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.AddInMemoryCollection([
                        new KeyValuePair<string, string?>("ExchangeRateProvider:MaxCurrencies", "2")
                    ]);
                });
            });

            var client = cfgFactory.CreateClient();
            var res = await client.GetAsync("/api/ExchangeRates?currencyCodes=USD,EUR,GBP");
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }
    }
}


