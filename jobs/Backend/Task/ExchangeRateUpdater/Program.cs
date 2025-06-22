using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.Client;
using ExchangeRateService.Provider;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IList<Currency> currencies = new[]
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

        public async static Task Main(string[] args)
        {

            var services = new ServiceCollection();
            
            services.AddLogging();
            
            services.AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>();
            services.AddTransient<IExchangeRateCache, InMemmoryERCache>();
            
            services.AddRefitClient<ICNBRefitClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.cnb.cz"));
            
            var sp = services.BuildServiceProvider();
            
            try
            {
                var provider = sp.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies, DateTime.Now);

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            // Console.ReadLine();
        }
    }
}