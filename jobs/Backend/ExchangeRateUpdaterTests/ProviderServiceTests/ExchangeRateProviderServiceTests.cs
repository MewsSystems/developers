using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderServiceTests
    {
        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRates_FromMultipleFetchers()
        {
            // Arrange
            var provider = CreateProviderWithError(dailyFails: false, otherFails: false);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("CHF")
            };

            // Act
            var rates = await provider.GetExchangeRateAsync(currencies);

            // Assert
            Assert.NotEmpty(rates);
            Assert.Contains(rates, r => r.TargetCurrency.Code == "USD");
            Assert.Contains(rates, r => r.TargetCurrency.Code == "CHF");
        }

        [Fact]
        public async Task GetExchangeRateAsync_FallbackToCache_WhenFetchFails()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var provider = CreateProviderWithError(dailyFails: false, otherFails: false, memoryCache);

            var currencies = new[] { new Currency("USD") };

            // Prime the cache
            var initial = await provider.GetExchangeRateAsync(currencies);
            Assert.NotEmpty(initial);

            // Now simulate both fetchers failing
            var providerWithFailure = CreateProviderWithError(
                dailyFails: true,
                otherFails: true,
                memoryCache);

            // Act
            var fallbackResult = await providerWithFailure.GetExchangeRateAsync(currencies);

            // Assert
            Assert.NotEmpty(fallbackResult);
            Assert.Equal(initial.Count, fallbackResult.Count); // cache fallback works
        }

        [Fact]
        public async Task GetExchangeRateAsync_Throws_WhenNoCacheAndAllFetchersFail()
        {
            // Arrange
            var provider = CreateProviderWithError(dailyFails: true, otherFails: true);

            var requested = new[] { new Currency("USD") };

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(async() =>
            {
                await provider.GetExchangeRateAsync(requested);
            });
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

            var mockDailyLogger = new Mock<ILogger<DailyExchangeRateFetcher>>();
            var mockOtherLogger = new Mock<ILogger<OtherCurrencyExchangeRateFetcher>>();

            var dailyFetcher = new DailyExchangeRateFetcher(
                httpClient,
                Options.Create(new CzechBankSettings { DailyRatesUrl = "https://mock-daily" }), mockDailyLogger.Object);

            var otherFetcher = new OtherCurrencyExchangeRateFetcher(
                httpClient,
                Options.Create(new CzechBankSettings { OtherCurrencyRatesUrl = "https://mock-other" }), mockOtherLogger.Object);

            var fetchers = new List<IExhangeRateFetcher> { dailyFetcher, otherFetcher };

            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
            var parserLogger = loggerFactory.CreateLogger<CzechNationalBankTextRateParser>();
            var parser = new CzechNationalBankTextRateParser(5, parserLogger);

            var providerLogger = loggerFactory.CreateLogger<ExchangeRateProviderService>();

            return new ExchangeRateProviderService(
                fetchers,
                parser,
                memoryCache ?? new MemoryCache(new MemoryCacheOptions()),
                providerLogger
            );
        }
    }
}