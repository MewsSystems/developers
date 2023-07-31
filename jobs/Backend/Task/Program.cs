using ExchangeRateLayer.BLL.Objects;
using ExchangeRateLayer.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateLayer.UI
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

        public static void Main(string[] args)
        {
            try
            {
                var provider = new ServiceManager();
                var rates = provider.ExchangeRateService.GetSelectedExchangeRates(currencies);
                DisplayExchangeRates(rates);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        public static void DisplayExchangeRates(IEnumerable<ExchangeRate> rates)
        {
            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
    }
}
