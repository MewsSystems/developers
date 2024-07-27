using Asp.Versioning;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;

namespace ExchangeRateUpdater.WebAPI.Controllers.v1
{
    /// <summary>
    /// Responsible for handling any requests related to Exchange Rates
    /// </summary>
    [ApiVersion("1.0")]
    public class ExchangeRateController : CustomBaseController
    {
        private readonly ILogger<ExchangeRateController> _logger;
        private readonly IExchangeRateGetService _exchangeRateGetService;

        /// <summary>
        /// Constructor for creating insance of the ExchangeRate Controller
        /// </summary>
        /// <param name="logger">Serilog logger</param>
        public ExchangeRateController(ILogger<ExchangeRateController> logger, IExchangeRateGetService exchangeRateGetService)
        {
            _logger = logger;
            _exchangeRateGetService = exchangeRateGetService;
        }

        /// <summary>
        /// Gets a list of all exchange rates currently available from the Czech National Bank
        /// </summary>
        /// <returns>Returns a list of Exchange Rates with Source Currency, Target Currency, and Value</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Getting Exchange Rates from Exchange Rate Controller");
                var results = await _exchangeRateGetService.GetExchangeRates();

                _logger.LogInformation($"Successfully retrieved {results.Count()} Exchange Rates from exchange rate get service");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all exchange rates.");
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all exchange rates currently available from the Czech National Bank filtered down to match the list of currency codes provided by the user
        /// </summary>
        /// <param name="currencyCodes">List of currency codes used to filter results coming back from the bank repository</param>
        /// <returns>Returns a list of filtered Exchange Rates with Source Currency, Target Currency, and Value</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> Get([FromQuery] List<string> currencyCodes)
        {
            try
            {
                if (currencyCodes == null || currencyCodes.Count() == 0)
                {
                    _logger.LogWarning("No currency codes provided for Get Filtered Exchange Rates.");
                    return BadRequest("No currency codes provided for Get Filtered Exchange Rates.");
                }

                _logger.LogInformation("Getting Filtered Exchange Rates from Exchange Rate Controller");
                var results = await _exchangeRateGetService.GetFilteredExchangeRates(currencyCodes);

                if (!results.Any())
                {
                    _logger.LogInformation("No exchange rates found for the provided currency codes.");
                    return NotFound("No exchange rates found for the provided currency codes.");
                }

                _logger.LogInformation($"Successfully retrieved {results.Count()} filtered Exchange Rates from exchange rate get service");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting filtered exchange rates.");
                return Problem(ex.Message);
            }
        }
    }
}
