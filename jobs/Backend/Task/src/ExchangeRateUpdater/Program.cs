using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Middleware;

namespace ExchangeRateUpdater
{
    public static class Program
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

                // Get rates from all providers
                var allRates = await provider.GetExchangeRates(Currencies);
                var exchangeRates = allRates as ExchangeRate[] ?? allRates.ToArray();
                Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates from all providers:");
                foreach (var rate in exchangeRates)
                {
                    Console.WriteLine(rate.ToString());
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");

                // Get rates from specific provider using chain of responsibility
                try
                {
                    var czechRates = await repository.GetFromProviderAsync("CzechNationalBank", Currencies);
                    Console.WriteLine($"Successfully retrieved {czechRates["CzechNationalBank"].Length} exchange rates from CzechNationalBank:");
                    foreach (var rate in czechRates["CzechNationalBank"])
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Provider error: {e.Message}");
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");

                // List all available providers
                var allProvidersRates = await repository.GetAllAsync();
                Console.WriteLine($"Available providers: {string.Join(", ", allProvidersRates.Keys)}");
                foreach (var providerRates in allProvidersRates)
                {
                    Console.WriteLine($"{providerRates.Key}: {providerRates.Value.Length} rates");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
            await host.StopAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Register all application services using the modular DI approach
                    services.AddApplicationServices(hostContext.Configuration);
                });
    }
}
