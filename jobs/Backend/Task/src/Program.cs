using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExchangeRateUpdater
{
    public static class Program
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

        public static async Task<int> Main(string[] args)
        {
            // The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
            // logger configured in `UseSerilog()` below, once configuration and dependency-injection have both been
            // set up successfully.
            Log.Logger = CreateLoggerConfiguration().CreateLogger();

            try
            {
                Log.Information("Starting exchange provider console host.");

                using var host = CreateHostBuilder(args).Build();

                // MH: Notes
                // MH: Consider using autofac for and names services registar,
                // MH: Autofac also support property injection. Property injection is especially useful for ILogger 
                // MH: Consider using memory cache, some APIs have request limits (depends on the load)

                var currencyService = host.Services.GetService<ICurrencyRateService>();

                if (currencyService == null)
                    throw new Exception("Failed to resolve " + nameof(ICurrencyRateService) + " from IOC");

                var rates = await currencyService.GetCurrencyRatesAsync("CNB", currencies);
                rates = rates.ToList();

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }

                Console.ReadLine();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        private static LoggerConfiguration CreateLoggerConfiguration()
        {
            return new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Infinite,
                    rollOnFileSizeLimit: true)
#if DEBUG
                .WriteTo.Console();
#endif
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .AddHttpClient()
                        .AddSingleton<ICurrencyRateService, CurrencyRateService>())
                .UseSerilog();
        }
    }
}