using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

		private readonly HttpClient _httpClient;
		private readonly string _CommonCurrenciesUrl;
		private readonly string _OtherCurrenciesUrl;

		public ExchangeRateProvider(string commonCurrenciesUrl, string otherCurrenciesUrl)
		{
			if (string.IsNullOrEmpty(commonCurrenciesUrl))
				throw new ArgumentException("Common currencies URL cannot be null or empty.", nameof(commonCurrenciesUrl));
			if (string.IsNullOrEmpty(otherCurrenciesUrl))
				throw new ArgumentException("Other currencies URL cannot be null or empty.", nameof(otherCurrenciesUrl));

			_CommonCurrenciesUrl = commonCurrenciesUrl;
			_OtherCurrenciesUrl = otherCurrenciesUrl;
			_httpClient = new HttpClient();
		}

		private static List<ExchangeRate> GetRateFromInputFile(string content, IEnumerable<Currency> currencies, List<ExchangeRate> exchangeRates)
		{
			if (string.IsNullOrEmpty(content))
				throw new ArgumentException("Content cannot be null or empty.", nameof(content));

			string[] lines = content.Split('\n');
			foreach (string line in lines)
			{
				string[] parts = line.Split('|');
				if (parts.Length == 5 && parts[3].Length == 3)
				{
					if (currencies.FirstOrDefault(c => c.Code == parts[3]) != null)
					{
						int sourceAmount;
						decimal targetRate;
						if (!int.TryParse(parts[2], out sourceAmount) || !decimal.TryParse(parts[4], out targetRate))
						{
							continue;
						}
						var sourceCurrency = new Currency(parts[3]);
						var exchangeRate = new ExchangeRate(sourceCurrency, sourceAmount, targetRate);
						exchangeRates.Add(exchangeRate);
					}
				}
			}
			return exchangeRates;
		}

		private async Task<List<ExchangeRate>> FetchAndProcessRatesAsync(string url, IEnumerable<Currency> currencies, List<ExchangeRate> exchangeRates)
		{
			HttpResponseMessage response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				return GetRateFromInputFile(content, currencies, exchangeRates);
			}
			else
			{
				throw new Exception($"Failed to retrieve data from {url}");
			}
		}

		public async Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			if (currencies == null || !currencies.Any())
				return Enumerable.Empty<ExchangeRate>().ToList();
			var exchangeRates = new List<ExchangeRate>();
			exchangeRates = await FetchAndProcessRatesAsync(_CommonCurrenciesUrl, currencies, exchangeRates);
			if (currencies.Count() == exchangeRates.Count)
				return exchangeRates;
			exchangeRates = await FetchAndProcessRatesAsync(_OtherCurrenciesUrl, currencies, exchangeRates);
			return exchangeRates;
		}
	}
}
