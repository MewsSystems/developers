using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public static class Program
	{
		private static readonly IEnumerable<Currency> currenciesChunkOne = new[]
		{
			new Currency("USD"),
			new Currency("EUR"),
			new Currency("CZK"),
			new Currency("JPY"),
			new Currency("KES"),
			new Currency("RUB"),
			new Currency("THB"),
			new Currency("TRY"),
			new Currency("XYZ"),
			new Currency("HUF"),
			new Currency("AUD"),
			new Currency("BRL"),
		};
		
		private static readonly IEnumerable<Currency> currenciesChunkTwo = new[]
		{

			new Currency("BGN"),
			new Currency("CAD"),
			new Currency("CNY"),
			new Currency("DKK"),
			new Currency("HKD"),
			new Currency("ISK"),
			new Currency("XDR"),
			new Currency("INR"),
			new Currency("IDR"),
			new Currency("ILS"),
			new Currency("MYR"),
			new Currency("MXN"),
		};
		
		private static readonly IEnumerable<Currency> currenciesChunkThree = new[]
		{
			new Currency("NZD"),
			new Currency("NOK"),
			new Currency("PHP"),
			new Currency("PLN"),
			new Currency("RON"),
			new Currency("SGD"),
			new Currency("ZAR"),
			new Currency("KRW"),
			new Currency("SEK"),
			new Currency("CHF"),
			new Currency("GBP"),
			new Currency(""),
			new Currency(null),
			null,
		};

		public static async Task Main(string[] args)
		{
			
			bool useProviderWithCache = true;
			
			try
			{
				ExchangeRateProvider provider = GetExchangeRateProvider(useProviderWithCache);
				List<ExchangeRate> rates = new();
				
				Stopwatch watch = Stopwatch.StartNew();
				rates.AddRange(await provider.GetExchangeRatesAsync(currenciesChunkOne));
				rates.AddRange(await provider.GetExchangeRatesAsync(currenciesChunkTwo));
				rates.AddRange(await provider.GetExchangeRatesAsync(currenciesChunkThree));
				watch.Stop();
				
				Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates in {watch.ElapsedMilliseconds} milliseconds:");
				
				foreach (ExchangeRate rate in rates)
				{
					Console.WriteLine(rate.ToString());
				}
				
				Console.ReadLine();
	
			}
			catch (Exception e)
			{
				Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
			}
		}
		
		private static ExchangeRateProvider GetExchangeRateProvider(bool useCache)
		{
			IExchangeRateApi exchageRateApi = new CzechExchangeRateApi();
			
			if(useCache)
			{
				ICache<ExchangeRateDitributor> cache = new InMemoryExchangeRateCache();	
				TimeSpan cacheDuration = TimeSpan.FromMinutes(5);
				return new ExchangeRateProvider(exchageRateApi, cache, cacheDuration);
			}
			
			return new ExchangeRateProvider(exchageRateApi);
		}
	}
}
