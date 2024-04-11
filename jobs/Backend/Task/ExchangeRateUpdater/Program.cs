using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Services.Domain;
using ExchangeRateUpdater.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ExchangeRateUpdater
{
    internal class Program
    {
        private static IEnumerable<Currency> currencies = new[]
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

        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration).WriteTo.Console())
                .ConfigureServices((context, services) =>
                {
                    services.AddConfiguration(context.Configuration)
                            .AddInternalServices()
                            .AddRefitClient(context.Configuration)
                            .AddMemoryCache(); // in a real world solution I would implement a Redis cache instead

                })
                .Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var provider =
                        serviceScope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();

                    var rates = await provider.GetExchangeRates(currencies).ConfigureAwait(false);

                    logger.LogInformation("Successfully retrieved {rates} exchange rates", rates.Count());

                    foreach (var rate in rates)
                    {
                        logger.LogInformation(rate.ToString());
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Could not retrieve exchange rates: '{message}'", e.Message);
                }

                Console.ReadLine();
            }
        }
    }
}
