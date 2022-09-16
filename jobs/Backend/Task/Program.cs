using System;
using System.Linq;
using Model;
using ExchangeRateProvider;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateService();
                var rates = provider.GetExchangeRates(SupportedCurrencies.Currencies);

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
