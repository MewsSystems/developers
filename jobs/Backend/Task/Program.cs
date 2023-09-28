using ExchangeRateUpdater.Exchange_Providers.Comparers;
using ExchangeRateUpdater.Exchange_Providers.Interfaces;
using ExchangeRateUpdater.Exchange_Providers.Models;
using ExchangeRateUpdater.Exchange_Providers.Provider.CNB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> currencies = new[]
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
            var serviceProvider = Configuration();

            try
            {
                var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies);

                var foundCurrencies = rates.Select(r => r.SourceCurrency);
                
                // We store the not found in case we had another provider later on which could
                // supply the rest of the exchange rates we are looking for.
                var notFound = currencies.Except(foundCurrencies, new CurrencyEqualityComparer());

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

        private static IServiceProvider Configuration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                .SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appsettings.json")
                                                .Build();

            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddScoped<IExchangeRateMapper<CNB_Exchange_Rate>, ExchangeRateMapper_CNB>();
            serviceCollection.AddScoped<IExchangeRateProvider, ExchangeRateProvider_CNB>();
            serviceCollection.AddHttpClient();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
