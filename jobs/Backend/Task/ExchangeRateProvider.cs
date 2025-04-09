using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		private static async Task<string> FetchRates()
		{
			using HttpClient client = new HttpClient();
			string today = DateTime.Today.ToString("yyyy-MM-dd");
			string url = "https://api.cnb.cz/cnbapi/exrates/daily?date=" + today;

			try
			{
				HttpResponseMessage response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode == false)
				{
					throw new Exception($"Response code {response.StatusCode}");
				}
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"error while fetching rates: '{ex.Message}'");
			}
		}

		// checks for any currencies that could not be found and prints a warning
		private static void CheckMissedCurrencies(Dictionary<Currency, bool> currencies)
		{
			foreach (var currency in currencies)
			{
				if (currency.Value == false)
				{
					Tools.WriteLine($"WARNING: could not fetch \"{currency.Key.ToString()}\"", ConsoleColor.Yellow);
				}
			}
		}

		private static IEnumerable<ExchangeRate> ParseRates(string rawJson, Dictionary<Currency, bool> currencies)
		{
			try
			{
				var rates = new List<ExchangeRate>();
				using JsonDocument json = JsonDocument.Parse(rawJson);
				IEnumerable<JsonElement> ratesArray = json.RootElement.GetProperty("rates").EnumerateArray();

				foreach (JsonElement rate in ratesArray)
				{
					Currency targetCurrency = new Currency(rate.GetProperty("currencyCode").GetString());
					if (currencies.Keys.Contains(targetCurrency) == false)
						continue;
					Currency sourceCurrency = new Currency("CZK");
					decimal totalRate = (decimal)(rate.GetProperty("rate").GetDouble() * rate.GetProperty("amount").GetInt64());

					rates.Add(new ExchangeRate(sourceCurrency, targetCurrency, totalRate));
					currencies[targetCurrency] = true;
				}
				return rates;
			}
			catch (Exception ex)
			{
				throw new Exception($"error while parsing rates: '{ex.Message}'");
			}
		}

		public IEnumerable<ExchangeRate> GetExchangeRates(Dictionary<Currency, bool> currencies)
		{
			var rawJson = FetchRates();
			rawJson.Wait();
			IEnumerable<ExchangeRate> rates = ParseRates(rawJson.Result, currencies);
			CheckMissedCurrencies(currencies);
			return rates;
		}
	}
}
