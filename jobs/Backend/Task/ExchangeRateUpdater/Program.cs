using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IReadOnlyCollection<Currency> _currencies = new[]
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

        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IExchangeRateProvider, ExchangeRateProvider>()
                .AddHttpClient<ICzechNationalBankExchangeRateGateway, CzechNationalBankExchangeRateGateway>(x =>
                    x.BaseAddress = new Uri("https://api.cnb.cz/"))
                .Services
                .BuildServiceProvider();

            try
            {
                var provider = serviceProvider.GetService<IExchangeRateProvider>();
                var rates = provider.GetExchangeRates(_currencies);

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

            Console.ReadLine();
        }
    }
}