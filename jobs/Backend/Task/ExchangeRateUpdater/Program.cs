using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.Client;
using ExchangeRateService.Client.Interfaces;
using ExchangeRateService.Client.Interfaces.CNB;
using ExchangeRateService.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using Serilog;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IList<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
        };

        public async static Task Main(string[] args)
        {

            var services = new ServiceCollection();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();
            
            services.AddLogging(b =>
            {
                b.ClearProviders();
                b.AddSerilog(Log.Logger);
            });
            
            services.AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>();
            services.AddSingleton<IExchangeRateCache, InMemmoryERCache>();
            
            // services.AddRefitClient<ICNBRefitClient>()
                // .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.cnb.cz"));
            
            services.AddTransient<ICNBClient, CNBClient>();
                
            var sp = services.BuildServiceProvider();
            
            try
            {
                var provider = sp.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies, DateTime.Now.AddMonths(-1));

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }
            
            await Log.CloseAndFlushAsync();

            // Console.ReadLine();
        }
    }
}