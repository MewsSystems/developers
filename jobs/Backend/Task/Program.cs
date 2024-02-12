using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class Program
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
                var builder = new HostBuilder()
                   .ConfigureServices((hostContext, services) =>
                   {
                       services.AddHttpClient();
                       services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
                       services.AddSingleton<ILogger>(x => CreateLogger());
                       services.AddSingleton<ExchangeRateProvider>();
                       services.AddSingleton<IExchangeRateProvider>
                            (x => new ExchangeRateProviderWithCaching(x.GetRequiredService<ExchangeRateProvider>()));
                   })
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.AddConsole();
                   })
                   .UseConsoleLifetime();
               
                var host = builder.Build();
                var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies);

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

        private static ILogger CreateLogger()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            return logger;
        }
    }
}
