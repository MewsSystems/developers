using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace ExchangeRateUpdaterTests.StartupTests
{
    public class StartupTests
    {
        [Fact]
        public void ConfigureServices_RegistersDependenciesCorrectly()
        {
            var startup = new Startup();
            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            var providerService = provider.GetRequiredService<IExchangeRateProviderService>();
            Assert.NotNull(providerService);

            var cache = provider.GetRequiredService<IMemoryCache>();
            Assert.NotNull(cache);

            var settings = provider.GetRequiredService<IOptions<CzechBankSettings>>();
            Assert.NotNull(settings);
            Assert.False(string.IsNullOrWhiteSpace(settings.Value.DailyRatesUrl));
            Assert.False(string.IsNullOrWhiteSpace(settings.Value.OtherCurrencyRatesUrl));
            Assert.True(settings.Value.TimeoutSeconds > 0);
            Assert.True(settings.Value.RetryCount >= 0);

            var parser = provider.GetRequiredService<IExchangeRateParser>();
            Assert.NotNull(parser);

            var fetchers = provider.GetServices<IExhangeRateFetcher>().ToList();
            Assert.NotEmpty(fetchers);

            bool hasDaily = fetchers.Any(f => f is DailyExchangeRateFetcher);
            bool hasOther = fetchers.Any(f => f is OtherCurrencyExchangeRateFetcher);

            Assert.True(hasDaily);
            Assert.True(hasOther);

            var httpFactory = provider.GetRequiredService<IHttpClientFactory>();
            Assert.NotNull(httpFactory);

            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);

            var logger = loggerFactory.CreateLogger<StartupTests>();
            Assert.NotNull(logger);
        }

        [Fact]
        public async Task ConfigureServices_Throws_WhenUrlsAreMissing()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>())
                .Build();

            var services = new ServiceCollection();

            var startup = new StartupForTest(config);
            startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            // Act + Assert: provider resolution should throw due to missing URLs
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = provider.GetRequiredService<IExchangeRateProviderService>();
            });
        }
    }
}
