using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Domain.Models;
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
                var rates = await provider.GetExchangeRates(Currencies);

                var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
                Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates:");
                foreach (var rate in exchangeRates)
                {
                    Console.WriteLine(rate.ToString());
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
                    services.AddDistributedMemoryCache();
                });
    }
}
