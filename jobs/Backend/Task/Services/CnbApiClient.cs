using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models.External;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Services
{
    public class CnbApiClient(
        HttpClient httpClient,
        ApiConfiguration configuration,
        ILogger<CnbApiClient> logger,
        IDateTimeSource dateTimeSource)
        : IApiClient<CnbRate>
    {
        public async Task<IEnumerable<CnbRate>> GetExchangeRatesAsync()
        {
            var queryParams = new Dictionary<string, string>
            {
                ["date"] = dateTimeSource.UtcNow.ToString("yyyy-MM-dd"),
                ["lang"] = configuration.Language
            };

            var queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            var uri = $"{configuration.ExchangeRateEndpoint}?{queryString}";

            try
            {
                logger.LogInformation("Initiated exchange rate request to: [{Uri}]", uri);

                var response = await httpClient.GetFromJsonAsync<CnbExchangeResponse>(uri);

                return response?.Rates ?? [];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving exchange rates from [{Uri}]", uri);
                throw;
            }
        }
    }
}
