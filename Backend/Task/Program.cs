using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    static class Program
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

        static void Main(string[] args)
        {

            var cnbFxRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

            try
            {
                using (var client = new HttpClient())
                {
                    var provider = new ExchangeRateProvider(
                        client, // Passing client in the constructor to make the dependency explicit
                        cnbFxRatesUrl, 
                        new CNBExchangeRatesParser()
                    );

                    // Using await/async because I thought that if it was to be used somewhere
                    // where blocking the main thread is not a good idea this approach might come in handy.
                    // It's pointless here but being ready didn't cost me anything in this case:)
                    var rates = provider.GetExchangeRates(currencies).Result;

                    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }

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
