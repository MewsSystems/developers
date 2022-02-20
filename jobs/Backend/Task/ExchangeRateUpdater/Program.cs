using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Helper;
using Common.Interface;
using Common.Model;
using Common.Providers;

namespace ExchangeRateUpdater
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			try
			{
				// Get CNB provider.
				ProviderBuilder pb = new ProviderBuilder();
				IExchangeRateProvider provider = pb.GetProvider<CnbProvider>();
				
				// Define currencies.
				Currency[] currencies = {
					new Currency("USD"), new Currency("EUR"), new Currency("CZK"), new Currency("JPY"), new Currency("KES"),
					new Currency("RUB"), new Currency("THB"), new Currency("TRY"), new Currency("XYZ"), new Currency("IDR")
				};

				// Load requested rates.
				List<ExchangeRate> rates = (await provider.GetExchangeRatesAsync(currencies)).ToList();

				Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
				rates.ForEach(p => Console.WriteLine(p.ToString()));
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e.Message}");
			}

			Console.ReadLine();
		}
	}
}
