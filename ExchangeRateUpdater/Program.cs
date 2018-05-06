﻿using System;
using System.Collections.Generic;
using System.Linq;
using Refit;

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
                IExchangeRateProvider provider = new ExchangeRateProvider(
                    RestService.For<ICurrencyConverterApi>("http://free.currencyconverterapi.com"), new Currency("CZK"),
                    new ExchangeRateParser());
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine("Successfully retrieved " + rates.Count() + " exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while retrieving exchange rates: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
