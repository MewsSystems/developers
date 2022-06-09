using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Tuple<Currency, Currency>> currencies = new []
        {
            new Tuple<Currency, Currency>(new Currency("EUR"), new Currency("USD")),
            new Tuple<Currency, Currency>(new Currency("EUR"), new Currency("JPY")),
            new Tuple<Currency, Currency>(new Currency("AUD"), new Currency("USD")),
            new Tuple<Currency, Currency>(new Currency("GBP"), new Currency("PLN")),
            new Tuple<Currency, Currency>(new Currency("CHF"), new Currency("EUR"))
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
