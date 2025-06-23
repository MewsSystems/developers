using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.ConsoleUI;

internal class ConsoleExecutor
{
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

	public static ConsoleExecutor Default => new();

	public async Task ExecuteAsync(IServiceProvider services)
	{
		try
		{
			var exchangeRateProvider = services.GetRequiredService<IExchangeRateProvider>();
			using var cts = new CancellationTokenSource(Debugger.IsAttached ? TimeSpan.FromMinutes(10) : TimeSpan.FromSeconds(20));
			var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies, cts.Token);

			var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
			Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");
			foreach (var rate in exchangeRates)
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