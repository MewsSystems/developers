using ExchangeRateProviderService.Services;
using ExchangeRateUpdaterModels.Models;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                var exchangeRates = await _dataRetrievalClient.GetExchangeRatesAsync(currencies);

                Log.Information($"Filtered exchange rates to {exchangeRates.Count()} items");

                return Ok(exchangeRates);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error fetching exchange rates from CNB API");
                return NoContent();
            }
        }
    }
}
