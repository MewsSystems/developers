using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IExchangeRateProvider provider;
        private static IEnumerable<Currency> currencies => new InMemoryReadOnlyCurrenciesRepository().GetAll();
        private static IRetryPoliciesBuilder retryPoliciesBuilder => new RetryPoliciesBuilder();

        public static async Task Main(string[] args)
        {
            try
            {
                using var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
                var logger = loggerFactory.CreateLogger<CzechNationalBankExchangeRateProvider>();

                provider = new CzechNationalBankExchangeRateProvider(new RestClient(), retryPoliciesBuilder, logger);

                var rates = await provider.GetExchangeRatesAsync(currencies);

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
    }
}
