using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateError;
using ExchangeRateModel;
using ExchangeRateService.Cache;
using ExchangeRateService.CNB.Client;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.CNB.Provider;
using ExchangeRateService.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public static ServiceProvider CreateServiceProvider()
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
            services.AddSingleton<IExchangeRateCache, InMemoryERCache>();
            
            services.AddTransient<ICNBClient, CNBClient>();
                
            var sp = services.BuildServiceProvider();
            return sp;
        }
        
        public static async Task Main(string[] args)
        {
            var sp = CreateServiceProvider();

            do
            {
                Console.Write("Select currency: ");
                string? currency = Console.ReadLine();
                if (currency == "exit")
                    break;
                if (string.IsNullOrEmpty(currency) || currency.Length != 3)
                {
                    Console.WriteLine("Invalid currency input");
                    continue;
                }
                Console.Write("Select date in format yyyy-MM-dd: ");
                string? date = Console.ReadLine();
                if (string.IsNullOrEmpty(date) || date.Length != 10)
                {
                    Console.WriteLine("Invalid date format");
                    continue;
                }

                try
                {
                    DateTime dateTime = DateTime.Parse(date);

                    var provider = sp.GetRequiredService<IExchangeRateProvider>();
                    var rate = await provider.GetExchangeRate(new Currency(currency), dateTime);

                    Console.WriteLine($"Successfully retrieved exchange rate:");
                    Console.WriteLine(rate);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Couldn't parse date: {ex.Message}");
                }
                catch (ExchangeRateException e)
                {
                    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                }
            } while (true);

            await Log.CloseAndFlushAsync();
            Console.WriteLine("Thank you for using ExchangeRateUpdater.");
            
        }
    }
}