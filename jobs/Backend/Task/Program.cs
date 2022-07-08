using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Helpers.Interfaces;
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

        public async static Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IApiService, ApiService>()
                .AddScoped<IBankCurrencyService, BankCurrencyService>()
                .AddScoped<IDataModifyingService, DataModifyingService>()
                .BuildServiceProvider();

            var apiService = serviceProvider.GetService<IApiService>();
            var bankCurrencyService = serviceProvider.GetService<IBankCurrencyService>();
            var dataModifyingService = serviceProvider.GetService<IDataModifyingService>();

            try
            {
                var provider = new ExchangeRateProvider(apiService, dataModifyingService, bankCurrencyService);
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

            Console.ReadLine();
        }
    }
}
