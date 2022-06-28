using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Tuple<Currency, Currency>> currencies = new[]
        {
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("USD")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("EUR")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("CZK")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("JPY")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("KES")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("RUB")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("THB")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("TRY")),
            new Tuple<Currency, Currency>(new Currency("CZK"), new Currency("XYZ"))
        };

        public static void Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(currencies);

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
