using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ExchangeRateUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // Create a service collection and configure logging
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get the logger
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            IExchangeRateService exchangeRateService = new CnbExchangeRateService();
            IExchangeRateProvider exchangeRateProvider = new CnbExchangeRateProvider(exchangeRateService);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };

            try
            {
                var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

                foreach (var rate in exchangeRates)
                {
                    logger.LogInformation(rate.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching exchange rates.");
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Add logging
            services.AddLogging(configure => configure.AddSerilog());
        }
    }
}
