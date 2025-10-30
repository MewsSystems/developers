using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.config;
using Serilog;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = ConfigurationLoader.Load();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(config.GetLogLevel())
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                config.Validate();

                Log.Information("Starting exchange rate retrieval for currencies: {Currencies}", config.Currencies);

                // var provider = new ExchangeRateProvider();
                // var rates = provider.GetExchangeRates(currencies);
                //
                // Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                // foreach (var rate in rates)
                // {
                //     Console.WriteLine(rate.ToString());
                // }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to retrieve exchange rates");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
