using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static Dictionary<Currency, bool> getCurrencies(string path)
        {
            var currencies = new Dictionary<Currency, bool>();
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException($"cannot find \"{path}\"");
            }
			string content = File.ReadAllText(path);
			foreach (string currency in content.Split(new char[] {' ', ',', '|', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries))
            {
				currencies.Add(new Currency(currency), false);
			}
			Tools.WriteLine($"Successfully read {currencies.Count()} currencies from \"{path}\"", ConsoleColor.Green);
			return currencies;
		}

		public static void Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateProvider();
                var currencies = getCurrencies("currencies.txt");
				var rates = provider.GetExchangeRates(currencies);

				Tools.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:", ConsoleColor.Green);
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Tools.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.", ConsoleColor.Red);
            }
            Console.ReadLine();
        }
    }
}
