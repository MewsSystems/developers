using System.Net.Http.Json;
using ExchangeRateUpdater.Core.DTOs;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Services;

public class ExchangeRateHttpClient : IExchangeRateRepository
{
    private readonly IApiConfiguration _apiConfiguration;

    public ExchangeRateHttpClient(IApiConfiguration apiConfiguration)
    {
        _apiConfiguration = apiConfiguration;
    }
    
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        using var httpClient = new HttpClient();

        try
        {
            var apiResponse = await httpClient.GetFromJsonAsync<ApiResponseDto>(_apiConfiguration.ApiUrl)
                ?? throw new ExchangeRateUpdaterException($"Null response from API {_apiConfiguration.ApiUrl}");
            return MapFromResponse(apiResponse);
        }
        catch (Exception e)
        {
            throw new ExchangeRateUpdaterException(e.Message, e);
        }
    }

    private IEnumerable<ExchangeRate> MapFromResponse(ApiResponseDto apiResponse)
    {
        return apiResponse.Rates.Select(MapFromRateInfoDto);
    }

    private ExchangeRate MapFromRateInfoDto(RateInfoDto rate)
    {
        return new ExchangeRate(rate.CurrencyCode, "CZK", rate.Rate / rate.Amount);
    }
}