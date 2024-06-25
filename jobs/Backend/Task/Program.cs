using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater
{
    public class Program
    {
        private static readonly IEnumerable<Currency> GetCurrencies = new[]
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
            var serviceProvider = ServiceProviderBuilder.Build();
            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            var logger = serviceProvider.GetService<ILogger<Program>>();

            try
            {
                var rates = await provider.GetExchangeRatesAsync(GetCurrencies);

                logger.LogError($"Successfully retrieved {rates.Count()} exchange rates:");

                foreach (var rate in rates)
                    Console.WriteLine(rate.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not retrieve exchange rates: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
