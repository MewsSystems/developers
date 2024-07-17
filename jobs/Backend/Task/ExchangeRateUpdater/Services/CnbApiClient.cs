using ExchangeRateUpdater.Models;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Services;

/// <summary>
/// This service is in charge of getting the data from the bank's API and then cacheing it until it is not longer valid.
/// </summary>
/// <remarks>
/// Found swagger docs here: https://api.cnb.cz/cnbapi/swagger-ui.html
/// </remarks>
public class CnbApiClient : ICnbApiClient
{
    private readonly HttpClient _httpClient;

    private DateTime? _validUntil = null;
    private ConcurrentDictionary<string, DailyExRateItem> _dailyExRateItems = new();

    private const string ExchangeRateEndpoint = "/cnbapi/exrates/daily";

    public CnbApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IDictionary<string, DailyExRateItem>> GetExchangeRatesFromCnbApi(CancellationToken token)
    {
        if (_validUntil > DateTime.Today)
        {
            // This data is still relevant so there is no reason to waste time fetching.
            return _dailyExRateItems;
        }

        // By default this API uses now as the time. But there could be a `date` parameter
        DailyExRatesResponse rateResponse = await _httpClient.GetFromJsonAsync<DailyExRatesResponse>($"{ExchangeRateEndpoint}?lang=EN", token)
            ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "API did not contain a valid response.");

        _dailyExRateItems.Clear();
        foreach (var rate in rateResponse.Rates)
        {
            // Looking at the API it seems this date is the same for all entries. But just to be sure the smallest ValidFor date is used to tell whether the data is stale.
            _validUntil = _validUntil < rate.ValidFor ? _validUntil : rate.ValidFor;
            _dailyExRateItems.AddOrUpdate(rate.CurrencyCode, rate, (key, currentRate) => rate);
        }

        return _dailyExRateItems;
    }
}
