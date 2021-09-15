using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExchangeRateUpdater.Parsing;

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
                Trace.AutoFlush = true;
                Trace.Listeners.Add(new ConsoleTraceListener());

                var configurationProvider = new ConfigurationProvider();
                var factory = new ExchangeRateParserFactory();

                var parser = factory.CreateParser(configurationProvider.Parser);

                var provider = new ExchangeRateProvider(new HttpCommunicator(configurationProvider.BaseUrl, configurationProvider.DateFormat), parser);

                var rates = provider.GetExchangeRates(currencies).Result;

                Trace.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Trace.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
