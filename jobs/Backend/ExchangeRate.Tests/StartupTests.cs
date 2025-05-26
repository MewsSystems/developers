using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExchangeRateUpdater.HttpClients;
using System.Linq;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Common.Constants;
using ExchangeRateUpdater.ExchangeRate.Providers;
using ExchangeRateUpdater;

namespace ExchangeRate.Tests
{
    public class StartupTests
    {
        [Fact]
        public void ConfigureServices_Registers_ExchangeRateService()
        {
            // Arrange
            var services = new ServiceCollection();
            var startup = new Startup();

            // Act
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            // Assert
            var service = provider.GetService<IExchangeRateService>();
            Assert.NotNull(service);
        }

        [Fact]
        public void ConfigureServices_Registers_MemoryCache()
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var cache = provider.GetService<IMemoryCache>();
            Assert.NotNull(cache);
        }

        [Fact]
        public void ConfigureServices_Registers_Logger()
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var logger = provider.GetService<ILogger<Startup>>();
            Assert.NotNull(logger);
        }

        [Fact]
        public void ConfigureServices_Registers_HttpClient_For_CzechApiClient()
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var client = provider.GetService<ICzechApiClient>();
            Assert.NotNull(client);
        }

        [Fact]
        public void ConfigureServices_Registers_CnbRatesCache_KeyedServices()
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            // Try to resolve all registered ICnbRatesCache keyed services
            var dailyCache = provider.GetKeyedService<ICnbRatesCache>(AppConstants.DailyRatesKeyedService);
            var monthlyCache = provider.GetKeyedService<ICnbRatesCache>(AppConstants.MonthlyRatesKeyedService);

            Assert.NotNull(dailyCache);
            Assert.NotNull(monthlyCache);
        }

        [Fact]
        public void ConfigureServices_Registers_Options()
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var httpOptions = provider.GetService<IOptions<ExchangeRateUpdater.Configuration.HttpServiceSettings>>();
            var czechApiOptions = provider.GetService<IOptions<ExchangeRateUpdater.Configuration.CzechApiSettings>>();

            Assert.NotNull(httpOptions);
            Assert.NotNull(czechApiOptions);
        }
    }
}