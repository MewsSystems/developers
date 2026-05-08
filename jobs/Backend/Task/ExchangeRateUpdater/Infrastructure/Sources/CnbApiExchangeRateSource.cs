using ExchangeRateUpdater.Application.DTOs;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Sources
{
    /// <summary>
    /// Retrieves exchange rates from the CNB (Czech National Bank) API using a HTTP client.
    /// </summary>
    public class CnbApiExchangeRateSource : IExchangeRateSource
    {
        private readonly ILogger<CnbApiExchangeRateSource> _logger;
        private readonly CnbApiOptions _options;
        private readonly HttpClient _httpClient;

        public CnbApiExchangeRateSource
        (
            ILogger<CnbApiExchangeRateSource> logger,
            IOptions<CnbApiOptions> options,
            HttpClient httpClient
        )
        {
            _logger = logger;
            _options = options.Value;
            _httpClient = httpClient;
        }

        //Date is assumed to be validated



        /// <summary>
        /// Retrieves daily exchange rates from the CNB API
        /// </summary>
        /// <param name="date">
        /// Optional date in (yyyy-MM-dd) format. If not provided, retrieves the latest available rates
        /// Date is assumed to be already validated
        /// </param>
        /// <returns>
        /// A collection of ExchangeRateDTO objects representing exchange rates for the requested date
        /// </returns>
        /// <exception cref="ExchangeRateSourceException">
        /// Thrown when the CNB API request fails, times out, or the response cannot be deserialized
        /// </exception>
        public async Task<IEnumerable<ExchangeRateDTO>> GetDailyExchangeRates(string date = null)
        {
            try
            {
                string requestURL = date != null ? $"{_options.BaseUrl}exrates/daily?date={date}&lang=EN" : _options.DailyRatesPath;

                var response = await _httpClient.GetAsync(requestURL);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var daily = JsonSerializer.Deserialize<DailyExchangeRatesDTO>(json, 
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (daily == null || daily.Rates == null)
                {
                    throw new JsonException("Deserialized exchange rate data is null");
                }

                return daily.Rates.ToList();
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "API request timed out or was cancelled");
                throw new ExchangeRateSourceException("CNB API request timed out", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while calling CNB exchange rate API");
                throw new ExchangeRateSourceException("Failed to retrieve exchange rates from CNB API", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize CNB exchange rate response");
                throw new ExchangeRateSourceException("Invalid response format from CNB API", ex);
            }
        }
    }
}
