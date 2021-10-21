using ExchangeRateUpdater.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.MessageWriter;
using System.Configuration;

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
            Logger log = new (MessageType.Console);
            try
            {
                string resultFolder = ConfigurationManager.AppSettings["ResultFolderpath"];
                string logFolder = ConfigurationManager.AppSettings["LogFolderPath"];
                string resultFile = ConfigurationManager.AppSettings["ResultFileName"];


                ExchangeRateProvider provider = new (log);
                var rates = provider.GetExchangeRates(currencies);

                MessageWriter.MessageWriter writer = new(MessageType.File, resultFolder, resultFile);             

                log.LogInfo($"Successfully retrieved {rates.Count()} exchange rates");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                    writer.WriteMessage(rate.ToString());
                }
            }
            catch (Exception e)
            {
                log.LogError($"Could not retrieve exchange rates: '{e.Message}'.");

            }
            Console.WriteLine("The program has ended successfully.");
            Console.ReadLine();
        }
    }

}
