using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Console.Configuration;
using ExchangeRateUpdater.Console.Configuration.Logging;
using ExchangeRateUpdater.Models.Entities;
using Serilog;
using Serilog.Events;

namespace ExchangeRateUpdater.Console
{
    public static class Program
    {
        public static string ApplicationName = "ExchangeRateUpdater";
        private static Settings _settings;

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

        public static async Task Main(string[] args)
        {
            try
            {
                var logger        = new SerilogConfiguration().Create(LogEventLevel.Debug, "http://localhost:5341/");
                var configuration = ApplicationConfiguration.GetConfiguration();
                _settings = Settings.From(configuration);

                ExchangeRateProvider provider = GetProvider(logger);

                logger.Information("Calling GetExchangeRatesAsync with ExchangeRateProvider");
                var rates = await provider.GetExchangeRatesAsync(currencies);

                System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    System.Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            System.Console.ReadLine();
        }

        private static ExchangeRateProvider GetProvider(ILogger logger)
        {
            return new ExchangeRateProvider(
                new ExchangeRateProviderSettings(_settings.ExchangeRateBaseUrl,
                                                 _settings.TimezoneId,
                                                 _settings.ExchangeRateBaseCurrency,
                                                 _settings.MappingSettings.Delimiter,
                                                 _settings.MappingSettings.DecimalSeparator,
                                                 true),
                logger
            );
        }
    }
}
