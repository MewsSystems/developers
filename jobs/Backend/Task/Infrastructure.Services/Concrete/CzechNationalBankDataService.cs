using Infrastructure.Models.AppSettings;
using Infrastructure.Models.Constants;
using Infrastructure.Models.CzechNationalBankModels;
using Infrastructure.Models.Exceptions;
using Infrastructure.Models.Responses;
using Infrastructure.Services.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services.Concrete;

public class CzechNationalBankDataService : IBankDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly CzechNationalBankSettings _czechNationalBankSettings;

    public CzechNationalBankDataService(HttpClient httpClient, ILogger<CzechNationalBankDataService> logger, IOptions<CzechNationalBankSettings> czechNationalBankSettings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _czechNationalBankSettings = czechNationalBankSettings.Value;
    }

    public Currency GetDefaultCurrency()
    {
        return new Currency(_czechNationalBankSettings.DefaultCurrencyCode);
    }

    public async Task<List<CurrencyRateResponse>?> GetExchangeRates()
    {
        var response = await _httpClient.GetAsync($"{CzechNationalBankApiEndpoints.GetExchangeRates}?date={DateTime.Now:yyyy-MM-dd}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Request to the {response.RequestMessage?.RequestUri} did not return success. Returned status code: {response.StatusCode}.");
            throw new ApiRequestException("There was an issue with request to the Czech National Bank Api");
        }

        using var responseBody = await response.Content.ReadAsStreamAsync();
        var exchangeRateDailyResponse = await JsonSerializer.DeserializeAsync<ExchangeRateDailyResponse>(responseBody);
        
        if (exchangeRateDailyResponse == null || !exchangeRateDailyResponse.Rates.Any())
        {
            _logger.LogInformation($"{CzechNationalBankApiEndpoints.GetExchangeRates} returned success with empty data.");
            return null;
        }

        return exchangeRateDailyResponse.Rates;
    }
}
