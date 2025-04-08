using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net.Http;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

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
				throw new Exception($"error while fetching rates: {ex.Message}");
			}
		}

		private static IEnumerable<ExchangeRate> ParseRates(string rawJson)
		{
			var rates = new List<ExchangeRate>();

			try
			{
				using JsonDocument json = JsonDocument.Parse(rawJson);
				IEnumerable<JsonElement> ratesArray = json.RootElement.GetProperty("rates").EnumerateArray();

				foreach (JsonElement rate in ratesArray)
				{
					string code = rate.GetProperty("currencyCode").GetString();
					decimal totalRate = (decimal)(rate.GetProperty("rate").GetDouble() * rate.GetProperty("amount").GetInt64());

					rates.Add(new ExchangeRate("CZK", code, totalRate));
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"error while parsing rates: {ex.Message}");
			}
			return rates;
		}

		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			try
			{
				var rawJson = FetchRates();
				rawJson.Wait();
				IEnumerable<ExchangeRate> rates = ParseRates(rawJson.Result);
				return rates;

			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
			}
			return Enumerable.Empty<ExchangeRate>();
		}
	}
}
