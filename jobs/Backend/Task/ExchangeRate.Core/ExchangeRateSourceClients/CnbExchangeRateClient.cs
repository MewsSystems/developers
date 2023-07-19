using System.Text.Json;
using ExchangeRate.Core.Constants;
using ExchangeRate.Core.Exceptions;
using ExchangeRate.Core.Models.ClientResponses;

namespace ExchangeRate.Core.ExchangeRateSourceClients;

public class CnbExchangeRateClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CnbExchangeRateClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<CnbExchangeRateResponse>> GetExchangeRatesAsync(string urlPath)
    {
        if (string.IsNullOrWhiteSpace(urlPath))
        {
            throw new ArgumentNullException(nameof(urlPath));
        }

        var client = _httpClientFactory.CreateClient(ExchangeRateSourceCodes.CzechNationalBank);

        var response = await client.GetAsync(urlPath) 
            ?? throw new ExchangeRateSourceException($"{ExchangeRateSourceCodes.CzechNationalBank} exchange rate source returns empty response.");

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            var errorCode = response.StatusCode;

            throw new ExchangeRateSourceException($"{ExchangeRateSourceCodes.CzechNationalBank} exchange rate source returns {errorCode} with message: {errorText}");
        }

        var content = await response.Content.ReadAsStringAsync();

        var cnbResponse = JsonSerializer.Deserialize<IEnumerable<CnbExchangeRateResponse>>(content);

        return cnbResponse;
    }
}
