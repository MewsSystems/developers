using System;
using System.Collections.Generic;
using System.Linq;

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
            Uri sourceProviderURI = ParseURLOutOfArguments(args);

            try
            {
                var provider = new ExchangeRateProvider(sourceProviderURI);
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

        /// <summary>
        /// Parse URL method is accepting command line arguments (arrays of strings). Arguments are optional,
        /// URL is expected as first entry in the arguments, ignoring rest of the arguments.
        /// Provided URL must be valid URL and well formed, if it is not, null is returned and message is logged into console.
        /// </summary>
        private static Uri ParseURLOutOfArguments(string[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }
            
            string sourceProviderURLArgument = args[0].Trim();
            if (string.IsNullOrEmpty(sourceProviderURLArgument))
            {
                System.Console.WriteLine("Provided Exchange Rate Provider URL is containing only whitespaces or is empty. Please specify valid URL.");
                return null;
            }

            Uri sourceProviderURI = null;
            try
            {
                sourceProviderURI = new Uri(sourceProviderURLArgument.ToLowerInvariant());
            }
            catch (UriFormatException)
            {
                System.Console.WriteLine("Provided Exchange Rate Provider URL is not an URL. Please specify valid URL.");
                return null;
            }
            if (!sourceProviderURI.IsWellFormedOriginalString())
            {
                System.Console.WriteLine("Provided Exchange Rate Provider URL is invalid or contains unescaped characters. Please specify valid URL.");
                return null;
            }
            return sourceProviderURI;
        }
    }
}
