﻿using System;
using System.Collections.Generic;
using System.Linq;

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
                var provider = CreateExchangeRateProvider();
                var rates = provider.GetExchangeRates(Currencies).ToList();

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
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

        private static ExchangeRateProvider CreateExchangeRateProvider()
        {
            var url = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
            var dataSourceProvider = new RestExchangeRateDataSourceProvider(url);
            var deserializer = new CzechNationalBankExchangeRatesDeserializer(new CzechNationalBankExchangeRateDeserializer());

            return new ExchangeRateProvider(dataSourceProvider, deserializer);
        }
    }
}
