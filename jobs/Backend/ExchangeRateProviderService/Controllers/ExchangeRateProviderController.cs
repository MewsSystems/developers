using ExchangeRateProviderService.Services;
using ExchangeRateUpdaterModels.Models;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Serilog;
using System.Collections.Generic;

namespace ExchangeRateProviderService.Controllers
{
    /// <summary>
    /// Controller to handle exchange rate data requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateProviderController : ControllerBase
    {
        private readonly DataRetrievalClient _dataRetrievalClient;

        // Define Prometheus metrics
        private static readonly Counter TotalRequests = Metrics.CreateCounter("exchange_rate_provider_requests_total", "Total number of requests to the ExchangeRateProvider");
        private static readonly Histogram RequestDuration = Metrics.CreateHistogram("exchange_rate_provider_request_duration_seconds", "Histogram of request duration for the ExchangeRateProvider"); 
        private static readonly Counter ErrorsCount = Metrics.CreateCounter("exchange_rate_provider_errors_total", "Total number of errors in the ExchangeRateProvider");
        private static readonly Counter DependencyCalls = Metrics.CreateCounter("exchange_rate_provider_dependency_calls_total", "Total number of calls to dependencies in the ExchangeRateProvider");
        private static readonly Histogram DependencyCallDuration = Metrics.CreateHistogram("exchange_rate_provider_dependency_call_duration_seconds", "Histogram of dependency call duration for the ExchangeRateProvider"); 
        private static readonly Counter CurrencyConversions = Metrics.CreateCounter("exchange_rate_provider_currency_conversions_total", "Total number of currency conversions in the ExchangeRateProvider");
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateProviderController"/> class.
        /// </summary>
        /// <param name="dataRetrievalClient">The data retrieval client.</param>
        public ExchangeRateProviderController(DataRetrievalClient dataRetrievalClient)
        {
            _dataRetrievalClient = dataRetrievalClient;
        }

        /// <summary>
        /// Gets the exchange rates for the specified currencies.
        /// </summary>
        /// <param name="currencies">The list of currencies to get exchange rates for.</param>
        /// <returns>A list of exchange rates.</returns>

        [HttpPost("data")]
        public async Task<IActionResult> GetExchangeRates([FromBody] IEnumerable<CurrencyModel> currencies)
        {
            using (RequestDuration.NewTimer())
            {
                TotalRequests.Inc();
                try
                {
                    IEnumerable<ExchangeRateModel> exchangeRates =null;
                    using (DependencyCallDuration.NewTimer())
                    {
                        // Simulate a dependency call
                        DependencyCalls.Inc();
                         exchangeRates = await _dataRetrievalClient.GetExchangeRatesAsync(currencies);
                    }
                    Log.Information($"Filtered exchange rates to {exchangeRates.Count()} items");

                    // Simulate a currency conversion
                    CurrencyConversions.Inc();
                    return Ok(exchangeRates);
                }
                catch (Exception e)
                {
                    ErrorsCount.Inc();
                    Log.Error(e, "Error fetching exchange rates from CNB API");
                    return NoContent();
                }
            }
        }
    }
}
