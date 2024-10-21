using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExchangeRateUpdater.Infrastructure
{
	public static class Configuration
	{
		public const string ExchangeRatesHttpClient = "exchangeRates";
		public const string TargetCurrency = "CZK";
		public const string CnbExchangeRatesGetPath = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";
		public const int MaxRetries = 3;
		public const int RequestInterval = 2;

		static IEnumerable<Currency> _currencies = new[]
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

		public static IEnumerable<Currency> Currencies => _currencies;

		public static string LoggingPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles", $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}", "Log.txt");
	}
}
