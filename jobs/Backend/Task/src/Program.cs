using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IExchangeRateProvider provider = new CzechNationalBankExchangeRateProvider(new RestClient());
        private static IEnumerable<Currency> currencies => new InMemoryReadOnlyCurrenciesRepository().GetAll();

        public static async Task Main(string[] args)
        {
            try
            {
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
