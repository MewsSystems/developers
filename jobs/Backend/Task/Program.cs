using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public record CurrencyPair(
            Currency SourceCurrency,
            Currency TargetCurrency
        );
        private static IEnumerable<CurrencyPair> currencies = new[]
        {
            new CurrencyPair(new Currency("CZK"), new Currency("USD")),
            new CurrencyPair(new Currency("CZK"), new Currency("EUR")),
            new CurrencyPair(new Currency("CZK"), new Currency("CZK")),
            new CurrencyPair(new Currency("CZK"), new Currency("JPY")),
            new CurrencyPair(new Currency("CZK"), new Currency("KES")),
            new CurrencyPair(new Currency("CZK"), new Currency("RUB")),
            new CurrencyPair(new Currency("CZK"), new Currency("THB")),
            new CurrencyPair(new Currency("CZK"), new Currency("TRY")),
            new CurrencyPair(new Currency("CZK"), new Currency("XYZ"))
        };

        public static async Task Main(string[] args)
        {
            try
            {
                var rates = await ExchangeRateProvider.GetExchangeRatesAsync(currencies);

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
