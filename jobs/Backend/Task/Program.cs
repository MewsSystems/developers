using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;

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
            new Currency("XYZ")
        };

        public static void Main(string[] args)
        {
            try
            {
                var exchangeRateClient = new ExchangeRateClient();
                var provider = new CnbExchangeRateProvider(exchangeRateClient);
                provider.PrintExchangeRates(Currencies);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
