namespace ExchangeRateUpdater.Infrastructure.ApiClients;

using System.Text;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.ExchangeRates.Dtos;
using Domain.Common;
using Domain.Enums;

public class ExchangeRateApiClient : IExchangeRateApiClient
{
    private readonly HttpClient _httpClient;

    public ExchangeRateApiClient(HttpClient httpClient)
    {
        Ensure.Argument.NotNull(httpClient, nameof(httpClient));
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExchangeRateApiDto>> GetExchangeRatesAsync(DateTime? date, Language? language)
    {
        var endpointRute = BuildExchangeRateDailyEndpointPath(date, language);
        var response = await _httpClient.GetAsync(endpointRute);
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadAsStringAsync();
        var exchangeRates = JsonSerializer.Deserialize<ExchangeRateApiResponse>(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return exchangeRates!.Rates;
    }
    
    public async Task<List<ExchangeRateApiDto>> GetDefaultExchangeRatesForYearAsync(int year)
    {
        var endpointRute = BuildExchangeRateDailyYearEndpointPath(year);
        var response = await _httpClient.GetAsync(endpointRute);
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadAsStringAsync();
        var yearResponse = JsonSerializer.Deserialize<ExchangeYearRatesApiDto>(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return yearResponse!.Rates;
    }

    private string BuildExchangeRateDailyYearEndpointPath(int year) 
        => new StringBuilder("daily-year?year=" + year).ToString();
    
    private string BuildExchangeRateDailyEndpointPath(DateTime? date, Language? language)
    {
        var stringBuilder = new StringBuilder(string.Empty);
        
        stringBuilder.Append("daily");

        if (date is not null)
        {
            stringBuilder.Append($"?date={date:yyyy-MM-dd}");
        }

        if (language is not null)
        {
            stringBuilder.Append($"&lang={language}");
        }

        return stringBuilder.ToString();
    }
}