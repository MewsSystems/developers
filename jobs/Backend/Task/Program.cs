using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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

		public static async Task Main()
		{
			string commonCurrenciesUrl;
			string otherCurrenciesUrl;

			try
			{
				var configuration = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.Build();

				commonCurrenciesUrl = configuration["ExchangeRateSources:CommonCurrencies"];
				otherCurrenciesUrl = configuration["ExchangeRateSources:OtherCurrencies"];
			}
			catch (Exception e)
			{
				Console.WriteLine($"Could not read configuration: '{e.Message}'.");
				return;
			}


			try
			{
				var provider = new ExchangeRateProvider(commonCurrenciesUrl, otherCurrenciesUrl);
				var rates = await provider.GetExchangeRates(currencies);

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
		}
	}
}
