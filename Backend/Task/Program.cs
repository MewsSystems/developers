using ExchangeRateUpdaterV2.Contracts;
using ExchangeRateUpdaterV2.Models;
using ExchangeRateUpdaterV2.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterV2
{
    public static class Program
    {
        // used exclusively for IHttpClientFactory
        private static readonly ServiceProvider _serviceProvider = GetFilledServiceCollection.BuildServiceProvider();

        // it is possible to add currencies during the runtime
        private static readonly ICollection<Currency> _currencies = new List<Currency>();

        public static async Task Main(string[] args)
        {
            // nothing to process
            if(!args.Any())
            {
                Console.WriteLine("No parameter set");
                Console.ReadLine();

                Environment.Exit(0);
            }

            // get ISO 4217 codes of currencies from parameters
            foreach(var arg in args)
            {
                _currencies.Add(new Currency(arg));
            }

            // iterate on every exchange rate provided in the container
            foreach (var exchangeRateProvider in _serviceProvider.GetServices<IExchangeRateProvider>())
            {
                try
                {
                    var counter = 0;
                    await foreach (var rate in exchangeRateProvider.GetExchangeRates())
                    {
                        Console.WriteLine(rate.ToString());
                        counter++;
                    }

                    Console.WriteLine($"Successfully retrieved {counter} exchange rates from {exchangeRateProvider.Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred while processing currency rates from {exchangeRateProvider.Name}: '{e.Message}'.");
                }
            }

            Console.ReadLine();
        }

        private static IServiceCollection GetFilledServiceCollection => new ServiceCollection()
            .AddHttpClient()
            .AddTransient(_ => _currencies)
            .AddTransient<IExchangeRateProvider, CnbExchangeRateProvider>();
    }
}