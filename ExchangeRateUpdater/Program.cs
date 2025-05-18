using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.DataFetchers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Entry point for the Exchange Rate Parser application.
    /// Fetches and displays exchange rates for a predefined set of currencies.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Predefined list of currencies for which exchange rates will be fetched.
        /// </summary>
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

        /// <summary>
        /// Main method: fetches exchange rates for predefined currencies and prints them to the console.
        /// Handles exceptions by reporting errors to the user.
        /// </summary>
        public static void Main(string[] args)
        {
            try
            {
                IRemoteDataFetcher dataFetcher = new HttpDataFetcher();
                IParser parser = new TextParser();
                var provider = new ExchangeRateService(dataFetcher, parser);
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
