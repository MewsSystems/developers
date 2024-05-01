using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Serilog;

namespace ExchangeRateUpdater
{
    public class Program
    {
        private readonly HttpClientService _httpClientService;
        private readonly CacheService _cacheService;
        private readonly ExchangeRateService _exchangeRateService;
        private readonly DateTime currentDate = DateTime.UtcNow;
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

        public Program()
        {
            _httpClientService = new HttpClientService();
            _cacheService = new CacheService();
            _exchangeRateService = new ExchangeRateService(_httpClientService, _cacheService);
        }

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/exchangeRateUpdater.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var program = new Program();
            await program.Run();

            Log.CloseAndFlush();
            Console.ReadLine();
        }

        private async Task Run()
        {
            Log.Information("Starting the Exchange Rate Updater");
            Log.Information("The data for the current working day is available after 14:30 CEST");
            try
            {
                var rates = await _exchangeRateService.GetExchangeRatesAsync(currencies, currentDate);

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
            }
        }
    }
}
