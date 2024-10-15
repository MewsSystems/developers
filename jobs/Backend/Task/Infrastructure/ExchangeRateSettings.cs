using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Infrastructure
{
	public static class ExchangeRateSettings
	{
		public const string TargetCurrency = "CZK";
		public const string CnbExchangeRatesGetPath = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

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
	}
}
