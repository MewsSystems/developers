using ExchangeRateProvider.Contract.API;
using ExchangeRateProvider.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        //This would be in config in production
        private static readonly string apiUrl = "https://localhost:7243/ExchangeRate";
        private static readonly string apiKey = ""; //Empty as there is not auth for now

        public static void Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateProvider(apiUrl, apiKey);
                var rates = provider.GetExchangeRates(ExchageRateProviderApi.GetSupportedCurrencies());

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
