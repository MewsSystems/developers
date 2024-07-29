using ExchangeRateUpdater.Core.ServiceContracts;
using ExchangeRateUpdater.Core.ServiceContracts.CurrencySource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.WebAPI.Controllers.v1
{
    /// <summary>
    /// Controller to handle all currency sources currently accepted by the application
    /// </summary>
    public class CurrencySourceController : CustomBaseController
    {
        private readonly ILogger<CurrencySourceController> _logger;
        private readonly ICurrencySourceGetService _currencySourceGetService;

        /// <summary>
        /// Constructor for creating insance of the CurrencySourceController Controller
        /// </summary>
        /// <param name="logger">Serilog logger</param>
        /// <param name="currencySourceGetService">Service that obtains currency source information from the source repository</param>
        public CurrencySourceController(ILogger<CurrencySourceController> logger, ICurrencySourceGetService currencySourceGetService)
        {
            _logger = logger;
            _currencySourceGetService = currencySourceGetService;
        }

        /// <summary>
        /// Gets a list of all of the currency sources currently used in the application
        /// </summary>
        /// <returns>Returns a list of CurrencySourceResponse containing Currency Code and Source API URL</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("CurrencySourceController - Get all called.");
                var sourceCurrencies = await _currencySourceGetService.GetAllCurrencySources();

                _logger.LogInformation($"CurrencySourceController  - Get all - {sourceCurrencies.Count} found");
                return Ok(sourceCurrencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CurrencySourceController - An error occurred while getting all exchange rates.");
                return Problem(ex.Message);
            }

        }
    }
}
