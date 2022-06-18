using ExchangeRateUpdated.Service.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

        public static async Task Main(string[] args)
        {
            try
            {
                var builtHost = CreateHostBuilder(args).Build();
                var provider = builtHost.Services.GetRequiredService<IExchangeRateProvider>();
                var result = await provider.GetExchangeRatesAsync(currencies);

                if (!result.IsSuccess)
                {
                    Console.WriteLine("Couldn't retrive rates");
                }

                var rates = result.Value;
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {

            })
            .ConfigureServices((hostcontext, services) =>
            {
                services.AddSingleton<ICnbCsvParser, CnbCsvParser>();
                services.AddHttpClient<IExchangeRateProvider, CnbExchangeRateProvider>()
                    .ConfigurePrimaryHttpMessageHandler(c =>
                    {
                        return new HttpClientHandler()
                        {
                            UseCookies = false,
                            AutomaticDecompression = DecompressionMethods.All
                        };
                    });
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddDebug();
            });
    }
}
