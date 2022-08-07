using System;
using System.Collections.Generic;
using Domain.Entities;

namespace ExchangeRateUpdater.Host.Console
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
                // TODO [07/08/2022] AR - get the exchange rates given the currencies
                // loop over the exchange rates and write them in the console
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            }

            System.Console.ReadLine();
        }
    }
}
