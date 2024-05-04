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
				IEnumerable<ExchangeRate> rates = Enumerable.Empty<ExchangeRate>();

				using (ExchangeRateProvider provider = new ExchangeRateProvider())
				{
					rates = await provider.GetExchangeRates(currencies);
				}

				Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
				foreach (var rate in rates)
				{
					Console.WriteLine(rate.ToString());
				}
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
				Console.ResetColor();
			}

			Console.ReadLine();
		}
	}
}
