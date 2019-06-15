/*
Additional notes:

The only source of exchange rates for this task which I found is http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt.
That shows the exchange rate of foreign currency compared to CZK => hence the constants.
The current state of ExchangeRateProvider shows the main idea about how to solve the given task. For the real application there are some areas which could/should be improved, e.g.:
- separating the logic of reading data from web, better error handling, setting timeouts / number of retries
- I would be more paranoid and would check denni_kurz.txt more. One can check whether the format of the first two lines is the same and if the first line contains the expected date.
===
I created a new git account work this task. I will send the info about who am I via Djela.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		private const string ExchangeRateUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
		private const string TargetCurrencyCode = "CZK";

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
		/// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

			Currency targetCurrency = currencies
				.Where(x => string.Equals(x.Code, TargetCurrencyCode, StringComparison.OrdinalIgnoreCase))
				.First();

			string exchangeRatesString;
			using (WebClient webClient = new WebClient())
			{
				exchangeRatesString = webClient.DownloadString(ExchangeRateUrl);
			}

			// Prepare an instance of Czech CultureInfo, its needed later for correct decimal parsing
			CultureInfo czechCultureInfo = CultureInfo.CreateSpecificCulture("cs-CZ");

			string[] lines = exchangeRatesString.Trim().Split('\n');
			foreach (string line in lines.Skip(2)) // First two lines dont contain exchange rate info
			{
				string[] items = line.Split('|');

				if (items.Length != 5)
				{
					throw new Exception($"Incorrect line format `{line}`");
				}

				if (!int.TryParse(items[2], out int amount))
				{
					throw new Exception($"Incorrect amount number `{items[2]}`");
				}
				string code = items[3];
				if (!decimal.TryParse(items[4], NumberStyles.Number, czechCultureInfo, out decimal exchangeRateValue))
				{
					throw new Exception($"Incorrect exchange rate  `{items[4]}`");
				}

				Currency sourceCurrency = currencies
					.Where(x => string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase))
					.FirstOrDefault();

				if (sourceCurrency != null)
				{
					ExchangeRate exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, exchangeRateValue / amount);
					exchangeRates.Add(exchangeRate);
				}
			}

			return exchangeRates;
		}
	}
}
