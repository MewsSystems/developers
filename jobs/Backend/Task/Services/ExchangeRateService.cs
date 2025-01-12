using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService(HttpClient httpClient, IConfiguration configuration) : IExchangeRateService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly string _baseApiUrl = configuration["BaseApiUrl"] ?? throw new ArgumentNullException(nameof(configuration));

    /// <inheritdoc />
    public async Task<ExchangeRatesResponseModel> GetExchangeRatesAsync(DateTime? date = null, string language = "EN")
    {
        var formattedDate = (date ?? DateTime.Now).ToString("yyyy-MM-dd");
        var requestUri = $"{_baseApiUrl}/exrates/daily?date={formattedDate}&lang={language}";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await _httpClient.GetFromJsonAsync<ExchangeRatesResponseModel>(requestUri);
        
        if (response is null)
        {
            throw new HttpRequestException("Error fetching exchange rates: Response content is null.");
        }
        
        return response;
    }
}