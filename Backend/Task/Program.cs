using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

		// added a performance counter to check solution metrics
        public static void Main(string[] args)
        {
			var stopWatch = Stopwatch.StartNew();
            try
            {
				
				var provider = new ExchangeRateProvider();
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
			finally
			{
				stopWatch.Stop();
				Console.WriteLine($"elapsed: {stopWatch.ElapsedMilliseconds}");
			}

            Console.ReadLine();
        }
    }
}
