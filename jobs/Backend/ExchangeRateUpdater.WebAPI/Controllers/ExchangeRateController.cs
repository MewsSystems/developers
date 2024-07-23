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
        /// Gets exchange rate in
        /// </summary>
        /// <returns></returns>
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
