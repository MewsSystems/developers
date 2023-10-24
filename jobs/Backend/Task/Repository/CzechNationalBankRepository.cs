using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Repository.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Repository
{
	public class CzechNationalBankRepository : ICzechNationalBankRepository
	{
		private readonly IConfiguration _configuration;

		public CzechNationalBankRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IEnumerable<ExternalCurrencyRate>> FetchCurrencyRates()
		{
			var today = DateTime.Now.ToString("yyyy-MM-dd");
			var baseUrl = _configuration["CzechNationalBankExchangeRatesAPIUrl"];
			var url = $"{baseUrl}?date={today}";

			var client = new HttpClient();
			HttpResponseMessage httpResponse;

			try
			{
				httpResponse = await client.GetAsync(url);
			}
			catch (HttpRequestException e)
			{
				throw new Exception(ErrorMessages.FailedToFetchDataFromAPI, e);
			}

			if (!httpResponse.IsSuccessStatusCode)
			{
				throw new Exception($"{ErrorMessages.WrongResponseStatusCode} {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
			}

			var response = await httpResponse.Content.ReadAsStringAsync();
			var ratesResponse = JsonSerializer.Deserialize<ExternalCurrencyRateResponse>(response);

			return ratesResponse.Rates;
		}
	}
}
