using System.Net.Http.Json;
using CurrencyRateUpdater;

namespace ExchangeRateUpdater.Service;

public interface ICentralBankService
{
	Task<CurrencyRatesResponse> GetExchangeRateResponse();
}

public class CentralBankService : ICentralBankService
{
	IHttpClientFactory _httpClientFactory;

	public CentralBankService(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public async Task<CurrencyRatesResponse> GetExchangeRateResponse()
	{
		var httpClient = _httpClientFactory.CreateClient(name: "csk_bank_ex_rates_daily");

		var response = await httpClient.GetAsync(httpClient.BaseAddress);

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<CurrencyRatesResponse>();
			return result;
		}

		return new CurrencyRatesResponse(Rates: new());
	}
}