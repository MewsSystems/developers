using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Infra.Models;

namespace ExchangeRateUpdater.Infra.Http;

public class CnbHttpClient : ICnbHttpClient
{
    private readonly HttpClient _httpClient;

    public CnbHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<ExchangeRateResponse, HttpError>> GetExchangeRatesAsync()
    {
        var uri = new Uri($"{_httpClient.BaseAddress}exrates/daily?lang=EN");
        var httpResponseMessage = await _httpClient.GetAsync(uri);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<ExchangeRateResponse>();
            return Result<ExchangeRateResponse, HttpError>.Success(response!);
        }

        var errorResponse = await httpResponseMessage.Content.ReadAsStringAsync();
        return Result<ExchangeRateResponse, HttpError>.Failure(new HttpError(httpResponseMessage.StatusCode, errorResponse));
    }
}