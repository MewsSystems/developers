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
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;

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
                var repository = host.Services.GetRequiredService<IExchangeRateRepository>();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Starting exchange rate retrieval process");

                // Get rates from all providers
                logger.LogInformation("Fetching exchange rates from all providers for {CurrencyCount} currencies", Currencies.Count());
                var allRates = await provider.GetExchangeRates(Currencies);
                var exchangeRates = allRates as ExchangeRate[] ?? allRates.ToArray();
                
                logger.LogInformation("Successfully retrieved {RateCount} exchange rates from all providers", exchangeRates.Length);
                Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates from all providers:");
                foreach (var rate in exchangeRates)
                {
                    Console.WriteLine(rate.ToString());
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");

                // Get rates from specific provider using chain of responsibility
                try
                {
                    logger.LogInformation("Fetching exchange rates from CzechNationalBank provider");
                    var czechRates = await repository.GetFromProviderAsync("CzechNationalBank", Currencies);
                    var czechRateCount = czechRates["CzechNationalBank"].Length;
                    
                    logger.LogInformation("Successfully retrieved {RateCount} exchange rates from CzechNationalBank", czechRateCount);
                    Console.WriteLine($"Successfully retrieved {czechRateCount} exchange rates from CzechNationalBank:");
                    foreach (var rate in czechRates["CzechNationalBank"])
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
                catch (ArgumentException e)
                {
                    logger.LogError(e, "Provider error occurred while fetching CzechNationalBank rates");
                    Console.WriteLine($"Provider error: {e.Message}");
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");

                // List all available providers
                logger.LogInformation("Retrieving all available providers");
                var allProvidersRates = await repository.GetAllAsync();
                var providerNames = string.Join(", ", allProvidersRates.Keys);
                
                logger.LogInformation("Available providers: {ProviderNames}", providerNames);
                Console.WriteLine($"Available providers: {providerNames}");
                foreach (var providerRates in allProvidersRates)
                {
                    logger.LogDebug("Provider {ProviderName}: {RateCount} rates", providerRates.Key, providerRates.Value.Length);
                    Console.WriteLine($"{providerRates.Key}: {providerRates.Value.Length} rates");
                }

                logger.LogInformation("Exchange rate retrieval process completed successfully");
            }
            catch (Exception e)
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "Could not retrieve exchange rates: {ErrorMessage}", e.Message);
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
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
                    
                    // Configure OpenTelemetry logging
                    logging.AddOpenTelemetry(options =>
                    {
                        options.AddConsoleExporter();
                        options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService(serviceName: "ExchangeRateUpdater", serviceVersion: "1.0.0"));
                    });
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Configure OpenTelemetry tracing
                    services.AddOpenTelemetry()
                        .WithTracing(tracing => tracing
                            .AddHttpClientInstrumentation()
                            .AddConsoleExporter()
                            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "ExchangeRateUpdater", serviceVersion: "1.0.0")));

                    // Register all application services using the modular DI approach
                    services.AddApplicationServices(hostContext.Configuration);
                });
    }
}
