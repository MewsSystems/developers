using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            new Currency("XYZ")
        };

        static ExchangeRateProvider ComposeProvider()
        {
            var parserOptions = Options.Create(new ExchangeRateParserOptions());
            var parser = new ExchangeRateParser(parserOptions);
            var downloaderOptions = Options.Create(new ExchangeRateDownloaderOptions());
            var downloader = new ExchangeRateDownloader(downloaderOptions);
            return new ExchangeRateProvider(downloader, parser);
        }

        public static async Task Main(string[] args)
        {
            try
            {
                var provider = ComposeProvider();
                using var downloadTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var rates = await provider.GetExchangeRates(currencies, downloadTimeout.Token);

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
