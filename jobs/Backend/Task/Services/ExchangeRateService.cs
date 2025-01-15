using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService(
    HttpClient httpClient,
    IConfiguration configuration,
    TimeProvider timeProvider,
    ILogger<ExchangeRateService> logger)
    : IExchangeRateService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly string _baseApiUrl = configuration.GetValue<string>("BaseApiUrl") ?? throw new ArgumentNullException(nameof(configuration));

    /// <inheritdoc />
    public async Task<ExchangeRatesResponseModel> GetExchangeRatesAsync(DateTime? date = null, string language = "EN")
    {
        var formattedDate = (date ?? timeProvider.GetUtcNow().Date).ToString("yyyy-MM-dd");
        var requestUri = $"{_baseApiUrl}/exrates/daily?date={formattedDate}&lang={language}";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Call to {requestUri} was unsuccessful ({statusCode} - {phrase})",
                requestUri, (int)response.StatusCode, response.ReasonPhrase);

            throw new HttpRequestException("Error fetching exchange rates.");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<ExchangeRatesResponseModel>();
        }
        catch (JsonException ex)
        {
            throw new HttpRequestException("Error fetching exchange rates: Response content could not be deserialized.", ex);
        }
    }
}