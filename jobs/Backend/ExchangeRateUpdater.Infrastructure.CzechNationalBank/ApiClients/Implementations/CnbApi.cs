using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using Serilog;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.ApiClients.Implementations;

public class CnbApi : IApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public CnbApi(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<NonNullResponse<IEnumerable<RateDto>>> GetCentralBankRates(string date)
    {
        return await GeRates( $"exrates/daily?date={date}&lang=EN");

    }

    public async Task<NonNullResponse<IEnumerable<RateDto>>> GetOtherCurrenciesRates(string date)
    {
        return await GeRates( $"fxrates/daily-month?lang=EN&yearMonth={date}");
    }

    private async Task<NonNullResponse<IEnumerable<RateDto>>> GeRates(string url)
    {
        try
        {
            var cnbClient = _httpClientFactory.CreateClient("CzechNationalBankApi");

            var response = await cnbClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.Error("The api has responded with {code} while retrieving rates: {@response}",
                    response.StatusCode, response);
                return NonNullResponse<IEnumerable<RateDto>>.Fail(new List<RateDto>(), $"Api responded with code {response.StatusCode}");
            }

            var deserializedResponse = JsonSerializer.Deserialize<CnbApiRatesResponse>(await response.Content.ReadAsStringAsync());
            if (deserializedResponse?.Rates != null)
            {
                return NonNullResponse<IEnumerable<RateDto>>.Success(deserializedResponse.Rates);
            }
        }
        catch (Exception exception)
        {
            _logger.Error(exception, "Error while retrieving exchange rates");
        }

        return NonNullResponse<IEnumerable<RateDto>>.Fail(new List<RateDto>(), "Could not retrieve exchange rates");
    }

}