using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Middleware;

namespace ExchangeRateUpdater
{
    public class Program
    {
        private static readonly IEnumerable<Currency> Currencies =
        [
            new("USD"),
            new("EUR"),
            new("CZK"),
            new("JPY"),
            new("KES"),
            new("RUB"),
            new("THB"),
            new("TRY"),
            new("XYZ")
        ];

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                await host.StartAsync();
                var provider = host.Services.GetRequiredService<ExchangeRateProvider>();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Starting exchange rate retrieval process");

                // Get rates from all providers
                logger.LogInformation("Fetching exchange rates from all providers for {CurrencyCount} currencies", Currencies.Count());
                var allRates = await provider.GetExchangeRates(Currencies);
                var exchangeRates = allRates as ExchangeRate[] ?? allRates.ToArray();
                
                logger.LogInformation("Successfully retrieved {RateCount} exchange rates from all providers", exchangeRates.Length);
                foreach (var rate in exchangeRates)
                {
                    Console.WriteLine(rate.ToString());
                }
                logger.LogInformation("Exchange rate retrieval process completed successfully");
            }
            catch (Exception e)
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "Could not retrieve exchange rates: {ErrorMessage}", e.Message);
            }

            Console.ReadLine();
            await host.StopAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    
                    // Configure OpenTelemetry logging using middleware
                    logging.AddOpenTelemetryLogging(hostContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Configure OpenTelemetry services using middleware
                    services.AddOpenTelemetryServices(hostContext.Configuration);

                    // Register all application services using the modular DI approach
                    services.AddApplicationServices(hostContext.Configuration);
                });
    }
}
