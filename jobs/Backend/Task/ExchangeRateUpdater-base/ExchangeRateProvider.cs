using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider : IDisposable
	{
		private readonly HttpClient _httpClient;

		public ExchangeRateProvider()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(Environment.GetEnvironmentVariable("EXCHANGE_RATE_UPDATER_URL"))
			};
		}

		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			List<ExchangeRate> rates = new List<ExchangeRate>();

			foreach (Currency item in currencies)
			{
				var response = await this._httpClient.GetAsync($"/v1/ExchangeRates?TargetCurrency={item.Code}");

				if (response.IsSuccessStatusCode)
				{
					var currencyRates = await response.Content.ReadFromJsonAsync<IEnumerable<ExchangeRate>>();
					rates.AddRange(currencyRates);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Error.WriteLine($"An error has occured when sending the request for {item.Code}, error code: {response.StatusCode}");
					Console.ResetColor();
					continue;
				}
			}

			return rates;
		}

		public void Dispose()
		{
			_httpClient.Dispose();
		}
	}
}
