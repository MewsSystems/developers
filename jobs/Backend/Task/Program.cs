using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder().ConfigureServices((contex, services) =>
            {
                services.AddHttpClient("CNBHttpClient", client =>
                {
                    client.BaseAddress = new Uri("https://api.cnb.cz/");
                });
                services.AddMemoryCache();
                services.AddSingleton<ILogger>(s => s.GetService<ILogger<ExchangeRateService>>());
                services.AddScoped<IExchangeRateService, ExchangeRateService>();
                services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
            }).UseConsoleLifetime();

            var app = builder.Build();
            var provider = app.Services.GetService<IExchangeRateProvider>();

            await Run(provider);

            Console.ReadLine();
        }

        public static async Task Run(IExchangeRateProvider provider)
        {
            try
            {
                var rates = await provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }
        }
    }
}
