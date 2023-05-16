using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Client.Client;
using ExchangeRateUpdater.Client.Contracts;

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

        public static void Main(string[] args)
        {
            try
            {
                // This bit will retrieve the data.
                // Not exposed to our application normally. Being vendor specific.
                var client = new ProviderClient(new HttpClient());
                
                // The bit we would expose to the rest of our application. Independent from the vendor.
                var provider = new ExchangeRateProvider(client);
                
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
