namespace ExchangeRateUpdater.Infrastructure.ApiClients;

using System.Text;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.ExchangeRates.Dtos;
using Application.ExchangeRates.Query;
using Domain.Common;
using Domain.Enums;
using Domain.Repositories;

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
        return exchangeRates.Rates;
    }

    private string BuildExchangeRateDailyEndpointPath(DateTime? date, Language? language)
    {
        var stringBuilder = new StringBuilder(string.Empty);
        
        stringBuilder.Append("daily");

        if (date is not null)
        {
            stringBuilder.Append($"?date={date}");
        }

        if (language is not null)
        {
            stringBuilder.Append($"&lang={language}");
        }

        return stringBuilder.ToString();
    }
}