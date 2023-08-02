using System.Net.Http.Json;
using CnbApiClient.Model;
using Microsoft.Extensions.Logging;

namespace CnbApiClient;

// TODO Could be maintained separately as a nuget package
public class CnbApiClient
{
    // TODO Could be moved to configuration object (and loaded from appsettings.json)
    private const string ExchangeRatesUrl =
        "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CnbApiClient> _logger;

    public CnbApiClient(IHttpClientFactory clientFactory, ILogger<CnbApiClient> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Gets latest available exchange rates from CNB API.
    /// </summary>
    /// <exception cref="CnbApiClientException">Throws when operation is not successful</exception>
    public async Task<CnbExchangeRates> GetLatestExchangeRatesAsync()
    {
        try
        {
            using var httpClient = _clientFactory.CreateClient();
            var rates = await httpClient.GetFromJsonAsync<CnbExchangeRates>(ExchangeRatesUrl);
            if (rates == null)
            {
                throw new InvalidOperationException("Failed to get exchange rates from CNB API");
            }

            return rates;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new CnbApiClientException("CNB API request failed", e);
        }
    }
}