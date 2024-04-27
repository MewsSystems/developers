using ExchangeRateUpdater.Application;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateFinder.API.Controllers
{
    [ApiController]
    [Route("api/exchange-rates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(IExchangeRateService exchangeRateService, ILogger<ExchangeRateController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetExchangeRates([FromQuery]string sourceCurrency, string targetCurrencies)
        {
            try
            {
                string[] currencyCodes = targetCurrencies.Split(',');
                if (currencyCodes == null || currencyCodes.Length == 0)
                {
                    return BadRequest("At least one currency pair must be provided.");
                }

                var exchangeRates = await _exchangeRateService.GetExchangeRates(sourceCurrency, currencyCodes);

                return Ok(exchangeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
