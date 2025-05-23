using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdaterTests.ProviderServiceTests
{
    public class ExchangeRateProviderServiceTests
    {
        private const string DailySample =
            "Country|Currency|Amount|Code|Rate\n" +
            "Date 20.05.2025 #123\n" +
            "United States|dollar|1|USD|22,345\n";

        private const string OtherSample =
            "Country|Currency|Amount|Code|Rate|Source\n" +
            "Date 20.05.2025 #456\n" +
            "Switzerland|franc|1|CHF|25,123|SNB\n";

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsCombinedRates()
        {
            var provider = CreateProvider(DailySample, OtherSample);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("CHF")
            };

            var result = await provider.GetExchangeRateAsync(currencies);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.TargetCurrency.Code == "USD");
            Assert.Contains(result, r => r.TargetCurrency.Code == "CHF");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_FallsBackToCache_WhenDailyFails()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var provider = CreateProvider(
                dailyRatesResponse:
                    "Country|Currency|Amount|Code|Rate\nDate\nUnited States|dollar|1|USD|22,345\n",
                otherRatesResponse:
                    "Country|Currency|Amount|Code|Rate|Source\nDate\nSwitzerland|franc|1|CHF|25,123|SNB\n",
                memoryCache: memoryCache);

            var requested = new[] { new Currency("USD"), new Currency("CHF") };
            var firstResult = await provider.GetExchangeRateAsync(requested);
            Assert.Equal(2, firstResult.Count); // fills cache 1st time

            // Fail http call
            var providerWithFailure = CreateProviderWithError(
                dailyFails: true,
                otherFails: true,
                memoryCache: memoryCache // use the same cache
            );

            var fallbackResult = await providerWithFailure.GetExchangeRateAsync(requested);

            Assert.Equal(2, fallbackResult.Count); // cache fallback works
        }

        [Fact]
        public async Task GetExchangeRatesAsync_Throws_WhenBothSourcesFailAndNoCache()
        {
            var provider = CreateProviderWithError(dailyFails: true, otherFails: true);

            var requested = new[] { new Currency("USD") };

            await Assert.ThrowsAsync<ApplicationException>(() =>
                provider.GetExchangeRateAsync(requested));
        }

        private ExchangeRateProviderService CreateProviderWithError(bool dailyFails, bool otherFails, IMemoryCache memoryCache = null)
        {
            var mockHttp = new MockHttpMessageHandler();

            if (dailyFails)
                mockHttp.When("https://mock-daily").Respond(System.Net.HttpStatusCode.InternalServerError);
            else
                mockHttp.When("https://mock-daily").Respond("text/plain", "Country|Currency|Amount|Code|Rate\nDate\nUnited States|dollar|1|USD|22,345\n");

            if (otherFails)
                mockHttp.When("https://mock-other").Respond(System.Net.HttpStatusCode.InternalServerError);
            else
                mockHttp.When("https://mock-other").Respond("text/plain", "Country|Currency|Amount|Code|Rate|Source\nDate\nSwitzerland|franc|1|CHF|25,123|SNB\n");

            var httpClient = mockHttp.ToHttpClient();
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ExchangeRateProviderService>();
            var options = Options.Create(new CzechBankSettings
            {
                DailyRatesUrl = "https://mock-daily",
                OtherCurrencyRatesUrl = "https://mock-other"
            });

            return new ExchangeRateProviderService(httpClient, logger, options, memoryCache ?? new MemoryCache(new MemoryCacheOptions()));
        }


        private ExchangeRateProviderService CreateProvider(string dailyRatesResponse, string otherRatesResponse, IMemoryCache memoryCache = null)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("https://mock-daily")
                    .Respond("text/plain", dailyRatesResponse);

            mockHttp.When("https://mock-other")
                    .Respond("text/plain", otherRatesResponse);

            var httpClient = mockHttp.ToHttpClient();

            var logger = LoggerFactory.Create(builder => builder.AddConsole())
                                      .CreateLogger<ExchangeRateProviderService>();


            var options = Options.Create(new CzechBankSettings
            {
                DailyRatesUrl = "https://mock-daily",
                OtherCurrencyRatesUrl = "https://mock-other"
            });


            return new ExchangeRateProviderService(httpClient, logger, options, memoryCache ?? new MemoryCache(new MemoryCacheOptions()));
        }
    }
}
