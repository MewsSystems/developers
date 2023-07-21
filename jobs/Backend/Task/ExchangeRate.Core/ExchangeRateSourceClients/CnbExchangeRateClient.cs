using System.Text.Json;
using ExchangeRate.Core.Constants;
using ExchangeRate.Core.Exceptions;
using ExchangeRate.Core.Models.ClientResponses;

namespace ExchangeRate.Core.ExchangeRateSourceClients;

public class CnbExchangeRateClient : IExchangeRateSourceClient<CnbExchangeRate>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CnbExchangeRateClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Returns collection of the CNB exchange rates by url.
    /// </summary>
    /// <param name="urlPath">Relative Url for receiving exchange rates.</param>
    /// <returns> Returns collection of the CNB exchange rates by url.</returns>
    /// <exception cref="ArgumentNullException">Throws if urlPath attribute is null or empty.</exception>
    /// <exception cref="ExchangeRateSourceException">Throws if the response from the CNB source is null or not successful</exception>
    public async Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesAsync(string urlPath)
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

        var cnbResponse = JsonSerializer.Deserialize<CnbExchangeRateResponse>(content);

        return cnbResponse?.Rates ?? Enumerable.Empty<CnbExchangeRate>();
    }
}
