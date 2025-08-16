using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Dtos;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.Apis;

public class CnbExchangeRateApi : IExchangeRateApi
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbExchangeRateApi> _logger;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public CnbExchangeRateApi(HttpClient httpClient, ILogger<CnbExchangeRateApi> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<CnbExchangeRateResponseItem>> GetExchangeRatesAsync(DateOnly? date, Language? language)
    {
        var requestUri = CreateExchangeRateUri(date, language);

        _logger.LogInformation("GetExchangeRates - Created Exchange Rate Uri: {Uri}", requestUri);

        var response = await _httpClient.GetAsync(requestUri);        

        try
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var exchangeRates = JsonSerializer.Deserialize<CnbExchangeRateResponse>(content, jsonSerializerOptions);
            if (exchangeRates?.Rates is not null && (exchangeRates.Rates?.Any() ?? false))
            {
                _logger.LogDebug("GetExchangeRates successful: {Response}", content);
                return exchangeRates.Rates;
            }

            return Enumerable.Empty<CnbExchangeRateResponseItem>();
        }
        catch(HttpRequestException httpRequestException)
        {
            _logger.LogError(httpRequestException, "{Message}: {Response}", CreateHttpRequestExceptionMessage(response.StatusCode), await response.Content.ReadAsStringAsync());
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unknown error in GetExchangeRatesAsync: {Message}", exception.Message);
            throw;
        }
    }

    private static string CreateExchangeRateUri(DateOnly? date, Language? language)
    {
        string exchangeRateUri = "daily";

        if (date is not null && language is not null)
        {
            exchangeRateUri += $"?date={date:yyyy-MM-dd}&lang={language}";
        }
        else if (date is not null)
        {
            exchangeRateUri += $"?date={date:yyyy-MM-dd}";
        }
        else if (language is not null)
        {
            exchangeRateUri += $"?lang={language}";
        }

        return exchangeRateUri;
    }

    private static string CreateHttpRequestExceptionMessage(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.BadRequest:
            {
                return $"Bad Request in {nameof(GetExchangeRatesAsync)}";
            }
            case HttpStatusCode.NotFound:
            {
                return $"Not Found in {nameof(GetExchangeRatesAsync)}";
            }
            case HttpStatusCode.InternalServerError:
            {
                return $"Internal Server Error in {nameof(GetExchangeRatesAsync)}";
            }
            default:
                return $"Http Request error in {nameof(GetExchangeRatesAsync)}";
        }
    }
}
