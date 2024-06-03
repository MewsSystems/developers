using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank
{
    /// <summary>
    /// Client for interacting with the Czech National Bank's API to fetch daily exchange rates.
    /// API documentation: https://api.cnb.cz/cnbapi/swagger-ui.html
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CzechNationalBankClient"/> class with the specified dependencies.
    /// </remarks>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="czechNationalBankConfigMonitor">The monitor for Czech National Bank configuration options.</param>
    /// <param name="logger">The logger.</param>
    internal class CzechNationalBankClient(HttpClient httpClient, IOptionsMonitor<CzechNationalBankConfig> czechNationalBankConfigMonitor, ILogger<CzechNationalBankClient> logger) : ICzechNationalBankClient
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly IOptionsMonitor<CzechNationalBankConfig> _czechNationalBankConfigMonitor = czechNationalBankConfigMonitor ?? throw new ArgumentNullException(nameof(czechNationalBankConfigMonitor));
        private readonly ILogger<CzechNationalBankClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public async Task<CzechNationalBankDailyExchangeRateResponse> GetDailyExchangeRates(FetchCzechNationalBankDailyExchangeRateRequest request, CancellationToken cancellationToken)
        {
            var config = _czechNationalBankConfigMonitor.CurrentValue;
            var url = $"{config.BaseUrl}/{config.DailyExchangeRateEndpoint}?date={request.Date:yyyy-MM-dd}&lang={request.Language}";

            try
            {
                var exchangeRateResponse = await _httpClient.GetAsync(url, cancellationToken);
                exchangeRateResponse.EnsureSuccessStatusCode();
                var exchangeRateJsonResponse = await exchangeRateResponse.Content.ReadAsStringAsync(cancellationToken);
                var exchangeRateResponseData = JsonUtil.Deserialize<CzechNationalBankDailyExchangeRateResponse>(exchangeRateJsonResponse);
                return exchangeRateResponseData;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Daily Exchange rate fetch failed - {nameof(CzechNationalBankClient)}#{nameof(GetDailyExchangeRates)}: {e.Message}");
                throw new ExchangeRateUpdaterException($"Failed to fetch daily exchange rates from the provider: {e.Message}");
            }
            catch (System.Exception e)
            {
                _logger.LogError($"Unexpected error - {nameof(CzechNationalBankClient)}#{nameof(GetDailyExchangeRates)}: {e.Message}");
                throw;
            }
        }
    }
}
