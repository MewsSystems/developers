using System;
using System.Linq;
using Model;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateProvider;
using ExchangeRateProvider.Service;
using ExchangeRateProvider.Cache;
using Model.Entities;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                var serviceCollection = new ServiceCollection();
                var serviceProvider = GetServiceProvider(serviceCollection);

                var exchangeRateService = serviceProvider.GetService<IExchangeRateService>();

                var rates = exchangeRateService.GetExchangeRates(SupportedCurrencies.Currencies);

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



        public static ServiceProvider GetServiceProvider(IServiceCollection services)
        {
            return services
                .AddScoped<IExchangeRateService, ExchangeRateService>()
                .AddScoped<ICache<Currency, ExchangeRate>, ExchangeRateCache>()
                .BuildServiceProvider();
        }
    }
}
