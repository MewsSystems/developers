using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderIntegrationTests
    {
        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsDataFromRealApi()
        {
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var rates = await _provider.GetExchangeRates(currencies);
            
            Assert.NotNull(rates);
            Assert.NotEmpty(rates);
            
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging(builder => builder.AddConsole()); 
            services.AddTransient<ExchangeRateProvider>();
        }

    }
}