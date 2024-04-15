using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ConsoleApp
{
    internal class Program
    {
        private static readonly IEnumerable<Currency> _currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];

        static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", false, true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(conf => conf.AddSimpleConsole())
                .AddTransient<IExchangeRateProvider, ExchangeRateProvider>()
                .AddInfrastructure(config)
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
            var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

            logger.LogDebug("Starting application");
            await LoadTodayExchangeRates(logger, provider);

            logger.LogInformation("Press 'Enter' to close program.");
            Console.ReadLine();
        }

        private static async Task LoadTodayExchangeRates(ILogger<Program> logger, IExchangeRateProvider provider)
        {
            try
            {
                var rates = await provider.GetExchangeRatesAsync(date: null, _currencies);

                logger.LogInformation("Successfully retrieved {Count} exchange rates:", rates.Count());
                foreach (var rate in rates)
                {
                    logger.LogInformation("{Rate}", rate.ToString());
                }
            }
            catch (Exception e)
            {
                logger.LogError("Could not retrieve exchange rates: '{Message}'.", e.Message);
            }
        }
    }
}
