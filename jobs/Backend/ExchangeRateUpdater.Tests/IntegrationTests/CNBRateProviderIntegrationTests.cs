using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests.IntegrationTests
{
    public class CNBRateProviderIntegrationTests
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ICurrencyProvider _currencyProvider;

        public CNBRateProviderIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            var services = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                        .AddConsole();
                });

            services.Configure<ExchangeRateUpdaterConfiguration>(configuration);
            services.AddMemoryCache();
            services.AddSingleton<ICustomHttpClient, DummyHttpClient>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ICurrencyProvider, CurrencyProvider>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

            var serviceProvider = services.BuildServiceProvider();

            _exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
            _currencyProvider = serviceProvider.GetRequiredService<ICurrencyProvider>();
        }

        [Fact]
        public async Task GetExchangeRates_Returns()
        {
            var result = await _exchangeRateProvider.GetExchangeRates(_currencyProvider.Get(), DateTime.Now, CancellationToken.None);

            Assert.True(result.Any());
        }
    }
}
