using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Console;

internal class ConsoleExecutor
{
	public static ConsoleExecutor Default => new();
	private readonly IEnumerable<Currency> currencies = new[]
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

	public async Task ExecuteAsync(IServiceProvider services)
	{
		try
		{
			var exchangeRateProvider = services.GetRequiredService<IExchangeRateProvider>();
			var rates = exchangeRateProvider.GetExchangeRates(currencies);

			var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
			System.Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");
			foreach (var rate in exchangeRates)
			{
				System.Console.WriteLine(rate.ToString());
			}
		}
		catch (Exception e)
		{
			System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
		}

		System.Console.ReadLine();
	}
}