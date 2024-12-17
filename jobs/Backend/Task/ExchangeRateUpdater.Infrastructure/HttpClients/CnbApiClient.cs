using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.HttpClients.Config;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Infrastructure.HttpClients;

/// <summary>
/// Czech National Bank API client.
/// </summary>
public class CnbApiClient : ICnbApiClient
{
    private readonly HttpClient _httpClient;

    string ExchangeRatesRequestUrl => UrlSettings.CnbApiOperations.DailyExchangeRatesUrl;

    public CnbApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<CnbExchangeRates> GetExchangeRatesAsync(DateTime date)
    {
        var content = await GetAsync($"{ExchangeRatesRequestUrl}?date={date:yyyy-MM-dd}");

        return JsonConvert.DeserializeObject<CnbExchangeRates>(content)!; ;
    }

    /// <inheritdoc/>
    public async Task<CnbExchangeRates> GetTodayExchangeRatesAsync()
    {
        var content = await GetAsync(ExchangeRatesRequestUrl);

        return JsonConvert.DeserializeObject<CnbExchangeRates>(content)!;
    }

    private async Task<string> GetAsync(string requestUri)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var response = await _httpClient.SendAsync(httpRequestMessage);

        return response.IsSuccessStatusCode ?
            await response.Content.ReadAsStringAsync() :
            string.Empty;
    }
}
