using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		private static List<ExchangeRate> getRateFromInputFile(string content, IEnumerable<Currency> currencies, List<ExchangeRate> exchangeRates){
			if (exchangeRates == null)
				throw new ArgumentNullException(nameof(exchangeRates));
			if (string.IsNullOrEmpty(content))
				throw new ArgumentException("Content cannot be null or empty.", nameof(content));

			string[] lines = content.Split('\n');
			foreach (string line in lines){
				string[] parts = line.Split('|');
				if (parts.Length == 5 && parts[3].Length == 3){
					if (currencies.FirstOrDefault(c => c.Code == parts[3]) != null){
						Currency sourceCurrency = new Currency(parts[3]);
						int sourceAmount = int.Parse(parts[2]);
						decimal targetRate = decimal.Parse(parts[4]);
						ExchangeRate exchangeRate = new ExchangeRate(sourceCurrency, sourceAmount, targetRate);
						exchangeRates.Add(exchangeRate);
					}
				}
			}
			return exchangeRates;
		}

		public List<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies){
			if (currencies == null || !currencies.Any())
				return Enumerable.Empty<ExchangeRate>().ToList();
			List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
			string sourceCommonCurrencies = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
			string sourceOtherCurrencies = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";
			using HttpClient httpClient = new HttpClient();
			HttpResponseMessage response = httpClient.GetAsync(sourceCommonCurrencies).Result;
			if (response.IsSuccessStatusCode){
				string content = response.Content.ReadAsStringAsync().Result;
				exchangeRates = getRateFromInputFile(content, currencies, exchangeRates);
			}
			else
				throw new Exception($"Failed to retrieve data from {sourceCommonCurrencies}");
			if (currencies.Count() == exchangeRates.Count())
				return exchangeRates;
			response = httpClient.GetAsync(sourceOtherCurrencies).Result;
			if (response.IsSuccessStatusCode){
				string content = response.Content.ReadAsStringAsync().Result;
				exchangeRates = getRateFromInputFile(content, currencies, exchangeRates);
			}
			else
				throw new Exception($"Failed to retrieve data from {sourceOtherCurrencies}");
			return exchangeRates;
		}
	}
}
