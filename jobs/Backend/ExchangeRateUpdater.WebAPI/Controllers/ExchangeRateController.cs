using ExchangeRateUpdater.Core.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebAPI.Controllers
{
    /// <summary>
    /// Responsible for handling any requests related to Exchange Rates
    /// </summary>
    public class ExchangeRateController : CustomBaseController
    {
        private readonly ILogger<ExchangeRateController> _logger;

        /// <summary>
        /// Constructor for creating insance of the ExchangeRate Controller
        /// </summary>
        /// <param name="logger">Serilog logger</param>
        public ExchangeRateController(ILogger<ExchangeRateController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of exchange rates for a specified list of currency codes
        /// </summary>
        /// <param name="request">Get Request object made up of a list of currency codes to get exchange rates for 
        /// and an optional date parameter.  The current DateTime of the request will be used if the value is null.</param>
        /// <returns>Returns a list of exchange rates for CurrencyCodes in the request object. If there isn't a matching value from
        /// Czech National Bank that currency will be ignored and will not have a corresponding exchange rate.</returns>
        [HttpGet(Name = "GetExchangeRates")]
        public async Task<IActionResult> Get(ExchangeRateGetRequest? request)
        {
            try
            {
                _logger.LogInformation("Getting Exchange Rates from Exchange Rate Controller");
                return Ok("Initial Exchange Rate Controller Creation");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
