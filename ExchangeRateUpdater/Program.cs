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

		public static void Main(string[] args)
		{
			var rateProvider = new CubExchangeRateProvider(new MemoryCache());

			RunExample(rateProvider);
			RunExample(rateProvider);
			RunExample(rateProvider);

			Console.ReadKey();
		}

		private static void RunExample(IExchangeRateProvider provider)
		{
			Task.Run(() =>
			{
				try
				{
					var rates = provider.GetExchangeRates(currencies);
					PrintResult(rates);
				}
				catch (Exception e)
				{
					Console.WriteLine("An error occurred while retrieving exchange rates: " + e.Message);
				}
			});
		}

		private static void PrintResult(IEnumerable<ExchangeRate> rates)
		{
			foreach (var rate in rates)
				Console.WriteLine(rate.ToString());
			Console.WriteLine();
		}
    }
}
