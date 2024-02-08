using ExchangeRateUpdater.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using CnbApi.Models;

namespace CnbApi.Client;
public interface ICnbClient
{
    Task<CnbDailyRatesContainerDto> GetDailyExchangeRates(DateTime date);
}

public class CnbClient : ICnbClient
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    public CnbClient(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
    }

    public async Task<CnbDailyRatesContainerDto> GetDailyExchangeRates(DateTime date)
    {
        var url = _apiSettings.ExchangeRatesBaseUrl +
          $"{_apiSettings.DailyRatesEndpoint}?date={date:yyyy-MM-dd}&lang={_apiSettings.LanguageCode}";

        try
        {
            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            if (!response.IsSuccessStatusCode)
            {
                // Log the error or throw a more specific exception
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new HttpRequestException($"Failed to get daily exchange rates. StatusCode: {response.StatusCode}. Response: {errorContent}");
            }
            var content = await response.Content.ReadAsStringAsync();

            var bankRates = JsonConvert.DeserializeObject<CnbDailyRatesContainerDto>(content);

            if (bankRates == null)
            {
                // Log this situation or handle it accordingly
                throw new InvalidOperationException("Received null bank rates from the CNB API.");
            }
            return bankRates;
        }
        catch (Exception ex)
        {
            throw new ("Failed to get daily exchange rates", ex);
        }
    }
}


