using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using log4net.Config;

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

        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            try
            {
                var provider = CreateExchangeRateProvider();
                var rates = provider.GetExchangeRates(Currencies).ToList();
                
                var stringRates = rates.Aggregate(string.Empty, (s, r) => s + Environment.NewLine + r);
                Log.Info($"Successfully retrieved {rates.Count} exchange rates: {stringRates}");

            }
            catch (Exception e)
            {
                Log.Error($"Could not retrieve exchange rates: '{e.Message}'.", e);
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
