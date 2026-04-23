using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Repository.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Repository
{
	public class CzechNationalBankRepository : ICzechNationalBankRepository
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public CzechNationalBankRepository(IConfiguration configuration, HttpClient httpClient)
		{
			_configuration = configuration;
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<ExternalCurrencyRate>> FetchCurrencyRates()
		{
			var today = DateTime.Now.ToString("yyyy-MM-dd");
			var baseUrl = _configuration["CzechNationalBankExchangeRatesAPIUrl"];
			var url = $"{baseUrl}?date={today}";
			HttpResponseMessage httpResponse;

			try
			{
				httpResponse = await _httpClient.GetAsync(url);
			}
			catch (HttpRequestException e)
			{
				throw new Exception(ErrorMessages.FailedToFetchDataFromAPI, e);
			}

			if (!httpResponse.IsSuccessStatusCode)
			{
				// Should also log the exception
				var detailedErrorMessage = $"{ErrorMessages.WrongResponseStatusCode} {(int)httpResponse.StatusCode}: {httpResponse.ReasonPhrase}";
				if (httpResponse.StatusCode == HttpStatusCode.NotFound)
					throw new Exception(detailedErrorMessage);

				var errorContent = await httpResponse.Content.ReadAsStringAsync();
				var errorDetails = JsonSerializer.Deserialize<CzechNationalBankAPIErrorMessage>(errorContent);

				detailedErrorMessage = $"{detailedErrorMessage} - {errorDetails.ErrorCode}: {errorDetails.Description}";

				throw new Exception(detailedErrorMessage);
			}

			var response = await httpResponse.Content.ReadAsStringAsync();
			var ratesResponse = JsonSerializer.Deserialize<ExternalCurrencyRateResponse>(response);

			return ratesResponse.Rates ?? Enumerable.Empty<ExternalCurrencyRate>();
		}
	}
}