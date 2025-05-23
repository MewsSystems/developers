using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ExchangeRateUpdaterTests.StartupTests
{
    public class StartupTests
    {
        [Fact]
        public void ConfigureServices_RegistersDependenciesCorrectly()
        {
            // Arrange
            var startup = new Startup();

            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            // Act + Assert
            var exchangeRateProvider = provider.GetRequiredService<IExchangeRateProviderService>();
            Assert.NotNull(exchangeRateProvider);

            var memoryCache = provider.GetRequiredService<IMemoryCache>();
            Assert.NotNull(memoryCache);

            var options = provider.GetRequiredService<IOptions<CzechBankSettings>>();
            Assert.NotNull(options);
            Assert.False(string.IsNullOrWhiteSpace(options.Value.DailyRatesUrl));
            Assert.False(string.IsNullOrWhiteSpace(options.Value.OtherCurrencyRatesUrl));
        }

        [Fact]
        public void ConfigureServices_Throws_WhenUrlsAreMissing()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>())
                .Build();

            var services = new ServiceCollection();
            services.Configure<CzechBankSettings>(config.GetSection("CzechBankSettings"));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IExchangeRateProviderService, ExchangeRateProviderService>();

            var provider = services.BuildServiceProvider();

            // Act + Assert: provider resolution should throw due to missing URLs
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = provider.GetRequiredService<IExchangeRateProviderService>();
            });
        }
    }
}
