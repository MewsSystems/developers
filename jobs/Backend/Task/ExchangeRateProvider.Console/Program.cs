using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.CnbApi;
using ExchangeRateUpdater.Model;
using LazyCache;
using Microsoft.Extensions.Options;

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
            new Currency("")
        };

        public static async Task Main(string[] args)
        {
            try
            {
                var apiConfig = Options.Create(new CnbApiOptions());
                var cacheConfig = Options.Create(new CachingOptions());

                var provider = new ExchangeRateProvider(
                    new CachedCnbApiClient(
                        new CnbApiClient(new HttpClient(), apiConfig),
                        new CachingService(),
                        new ExchangeRateReleaseDatesProvider(TimeProvider.System, apiConfig),
                        cacheConfig));

                var cts = new CancellationTokenSource();

                var rates = await provider.GetExchangeRates(currencies, cts.Token);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {

                    Console.WriteLine(rate.ToString());
                }


                rates = await provider.GetExchangeRates(currencies, cts.Token);

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
