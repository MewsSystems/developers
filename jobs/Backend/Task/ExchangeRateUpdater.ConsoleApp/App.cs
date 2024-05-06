using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Interfaces;

namespace ExchangeRateUpdater.ConsoleApp;

public class App
{
	private readonly IExchangeRateProvider _exchangeRateProvider;

	public App(IExchangeRateProvider exchangeRateProvider)
	{
		_exchangeRateProvider = exchangeRateProvider;
	}

	public async Task Run(string[] args)
	{
		IEnumerable<Currency> currencies = new[]
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

		var dateToday = DateOnly.FromDateTime(DateTime.Now);
		var rates = await _exchangeRateProvider.GetExchangeRates(dateToday, currencies);

		Console.WriteLine($"Exchange rates for today ({dateToday}):");
		foreach (var rate in rates)
		{
			Console.WriteLine(rate.ToString());
		}
	}
}