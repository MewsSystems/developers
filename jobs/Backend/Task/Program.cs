using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Unity;
using Unity.Injection;

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
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify appSettings.json file as a command line parameter.");
                return;
            }

            var container = UnityContainerFactory.Create(args[0]);
            
            ConfigureLog4Net(container);

            try
            {
                var provider = container.Resolve<IExchangeRateProvider>();
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

        private static void ConfigureLog4Net(IUnityContainer container)
        {
            var config = container.Resolve<IConfigurationRoot>();
            var log4NetConfigFile = config.GetSection(ExchangeRateUpdaterAppSettingsKeys.Log4netConfigFile)?.Value;

            if (string.IsNullOrEmpty(log4NetConfigFile))
            {
                Console.WriteLine("Log4net configuration is missing.");
                return;
            }

            if (!File.Exists(log4NetConfigFile))
            {
                Console.WriteLine($"Log4net configuration {log4NetConfigFile} does not exist.");
                return;
            }

            XmlConfigurator.Configure(new FileInfo(log4NetConfigFile));
        }


    }
}
