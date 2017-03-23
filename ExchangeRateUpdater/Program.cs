using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> Currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("PLN"),
            new Currency("MYR"),
            new Currency("XYZ")
        };

        public static void Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(Currencies);

                var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
                Console.WriteLine("Successfully retrieved " + exchangeRates.Count() + " exchange rates:");
                foreach (var rate in exchangeRates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while retrieving exchange rates: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
