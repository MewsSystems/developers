using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var services = new ServiceCollection();

            services.AddHttpClient<IExchangeRateProvider, CnbExchangeRateProvider>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            var serviceProvider = services.BuildServiceProvider();

            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();

            try
            {
                var rates = await exchangeRateService.GetExchangeRatesAsync(currencies);

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
