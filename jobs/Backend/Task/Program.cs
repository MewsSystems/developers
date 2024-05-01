using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

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
            new Currency("XYZ"),
            new Currency("BRL") // Added
        };

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/exchangeRateUpdater.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Starting the Exchange Rate Updater");
            Log.Information("The data for the current working day is available after 14:30 CEST");

            try
            {
                var provider = new ExchangeRateProvider();
                var rates = await provider.GetExchangeRatesAsync(currencies);

                if (rates.Any())
                {
                    Log.Information("Successfully retrieved {Count} exchange rates", rates.Count());
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
                else
                {
                    Log.Warning("No exchange rates were retrieved.");
                }

            }
            catch (Exception e)
            {
                Log.Error(e, "Could not retrieve exchange rates");
            }
            finally
            {
                Log.Information("Ending the Exchange Rate Updater");
                Log.CloseAndFlush();
            }

            Console.ReadLine();
        }
    }
}
