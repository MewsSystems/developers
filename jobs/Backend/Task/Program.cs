using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                var provider = CreateExchangeRateProvider();

                var cancellationToken = default(CancellationToken);

                var rates = await provider.GetExchangeRatesAsync(currencies, cancellationToken).ToListAsync(cancellationToken);

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

            Console.ReadLine();
        }

        private static ExchangeRateProvider CreateExchangeRateProvider()
        {
            // DI and configuration setup would be here
            const bool shouldIncludeOtherCurrencies = true;
            var currencyRateProvider = new CnbCurrencyRateProvider(shouldIncludeOtherCurrencies);
            return new ExchangeRateProvider(currencyRateProvider);
        }
    }
}