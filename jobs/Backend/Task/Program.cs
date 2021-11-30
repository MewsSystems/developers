using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.BusinessLayer;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Interfaces;
using SimpleInjector;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly Container container;

        static Program()
        {
            container = new Container();

            container.Register<ICnbXmlSource, ExtendedCnbWebXmlRatesSource>();
            container.Register<ExchangeRateProvider, CnbXlmRatesProvider>();

            container.Verify();
        }
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
                var provider = container.GetInstance<ExchangeRateProvider>();
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
