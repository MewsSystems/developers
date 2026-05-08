using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<CurrencyModel> currencies = new[]
        {
            new CurrencyModel("USD"),
            new CurrencyModel("EUR"),
            new CurrencyModel("CZK"),
            new CurrencyModel("JPY"),
            new CurrencyModel("KES"),
            new CurrencyModel("RUB"),
            new CurrencyModel("THB"),
            new CurrencyModel("TRY"),
            new CurrencyModel("XYZ")
        };

        public static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddSimpleConsole();
                })
                .ConfigureServices(services =>
                {
                    services.AddHttpClient<ExchangeRateProvider>(client =>
                    {
                        client.BaseAddress = new Uri("https://api.cnb.cz/");
                        client.Timeout = TimeSpan.FromSeconds(10);

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    });
                })
                .Build();

            try
            {
                var provider = host.Services.GetRequiredService<ExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync(currencies);

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

            Console.ReadLine();
        }
    }
}
