using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Infrastructure.Dto;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Clients;

internal class CzechNationalBankExchangeRateClient : ICzechNationalBankExchangeRateClient
{
	private readonly HttpClient _httpClient;
	private readonly ILogger _logger;

	public CzechNationalBankExchangeRateClient(HttpClient httpClient, ILogger<CzechNationalBankExchangeRateClient> logger)
	{
		_httpClient = httpClient;
		_logger = logger;
	}

	public async Task<IEnumerable<ExchangeRate>> FetchExchangeRates(string language, DateOnly date)
	{
		var res = await _httpClient.GetAsync($"daily?date={date.ToString("yyyy-MM-dd")}&lang={language}");

		if (!res.IsSuccessStatusCode)
		{
			_logger.LogError("Error code when fetching exchange rates. {@Response}", res);
			throw res.StatusCode switch
			{
				HttpStatusCode.NotFound => new FetchExchangeRatesFailException(
					"Failed to fetch exchange rates. Ensure API is configured correctly."),
				_ => new FetchExchangeRatesFailException()
			};
		}

		try
		{
			var response = await res.Content.ReadFromJsonAsync<ExRateDailyResponse>();
			if (response is null) throw new FetchExchangeRatesFailException();
			_logger.LogInformation("Received exchange rates. {@Rates}", response);
			return response.Rates.Select(r =>
				new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), decimal.Divide(r.Rate, r.Amount)));
		}
		catch (FetchExchangeRatesFailException)
		{
			_logger.LogError("Unexpected response received when fetching exchange rates.");
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError("Unexpected error occurred when fetching exchange rates. {ex}", ex);
			throw new FetchExchangeRatesFailException("Unexpected error occurred when fetching exchange rates.", ex);
		}
	}
}