using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Clients
{
    /// <summary>
    /// Client responsible for fetching exchange rate data from the CNB API.
    /// </summary>
    public class CnbExchangeRateClient : ICnbExchangeRateClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CnbExchangeRateClient> _logger;

        public CnbExchangeRateClient(HttpClient httpClient, ILogger<CnbExchangeRateClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves exchange rate data from the CNB API for the given date.
        /// </summary>
        /// <param name="dateString">Date in **yyyy-MM-dd** format.</param>
        /// <returns>
        /// A *CnbRateResponse* object.
        /// </returns>
        public async Task<CnbRateResponse?> GetRatesAsync(string dateString)
        {
            var relativePath = $"/cnbapi/exrates/daily?date={dateString}";
            try
            {
                _logger.LogInformation("Requesting CNB rates for {Date} at {RelativePath}", dateString, relativePath);
                return await _httpClient.GetFromJsonAsync<CnbRateResponse>(relativePath);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching CNB rates for {Date}.", dateString);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching CNB rates for {Date}.", dateString);
                throw;
            }
        }
    }
}
