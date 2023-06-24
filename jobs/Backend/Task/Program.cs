using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateProvider.External;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.Services;

namespace ExchangeRateProvider
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
                var fixExchangeRateService = new ExchangeRateFixingService(
                    new CzechNationalBankExchangeRateClient(),
                    new ExchangeRateFixingHtmlParser());
                var otherCurrenciesService = new ExchangeRateOtherCurrenciesService(
                    new CzechNationalBankExchangeRateClient(),
                    new ExchangeRateOtherCurrenciesHtmlParser());

                // TODO: consider caching strategy
                //var cachedFixExchangeRateService =
                //    new CachedExchangeRateService(fixExchangeRateService, ttlPolicy: d => d.AddHours(1));
                //var cachedOtherCurrenciesService =
                //    new CachedExchangeRateService(otherCurrenciesService, ttlPolicy: d => d.AddDays(30));

                var czechBankExchangeRateService = new CzechNationalBankExchangeRateService(fixExchangeRateService, otherCurrenciesService);
                var targetCurrencyCode = "CZK";

                var provider = new Services.ExchangeRateProvider(targetCurrencyCode, czechBankExchangeRateService);

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
