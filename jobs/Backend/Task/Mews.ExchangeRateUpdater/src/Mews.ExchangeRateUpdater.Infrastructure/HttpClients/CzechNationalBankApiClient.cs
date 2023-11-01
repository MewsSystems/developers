using Mews.ExchangeRateUpdater.Domain.Entities;
using Mews.ExchangeRateUpdater.Infrastructure.HttpClients.Config;
using Newtonsoft.Json;

namespace Mews.ExchangeRateUpdater.Infrastructure.HttpClients;

/// <summary>
/// Czech National Bank API client.
/// </summary>
public class CzechNationalBankApiClient : ICzechNationalBankApiClient
{
    private readonly HttpClient _httpClient;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient"></param>
    public CzechNationalBankApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<CNBExchangeRates> GetTodayExchangeRatesAsync()
    {
        var requestUri = UrlsConfig.CzechNationalBankApiOperations.GetDailyExchangeRates();

        var content = await GetAsync(requestUri, false);

        var result = JsonConvert.DeserializeObject<CNBExchangeRates>(content)!;

        return result;
    }

    private async Task<string> GetAsync(string requestUri, bool ensureSuccessStatusCode = true)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var response = await _httpClient.SendAsync(httpRequestMessage);

        if (ensureSuccessStatusCode)
        {
            _ = response.EnsureSuccessStatusCode();
        }

        switch (response.IsSuccessStatusCode)
        {
            case true:
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            default:
                return string.Empty;
        }
    }
}
