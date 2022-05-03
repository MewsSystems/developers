using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
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
                //TODO: Setup Dependency injection
                CnbApiClient client = new CnbApiClient(new System.Net.Http.HttpClient());
                IExchangeRateRepository repository = new CnbExchangeRateRepository(client);
                IExchangeRateProvider provider = new CnbExchangeRateProvider(repository);

                //Fetch rates data
                await repository.Initialize();

                //Provider is ready to use
                var rates = provider.GetExchangeRates(currencies);

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
