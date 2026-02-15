using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController(
        IExchangeRateService exchangeRateService,
        ILogger<ExchangeRateController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates([FromQuery] string currencyCodes)
        {
            logger.LogInformation("Received request to fetch exchange rates for currencies: {CurrencyCodes}", currencyCodes);

            string[] codes = currencyCodes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            IEnumerable<ExchangeRate> exchangeRates = await exchangeRateService.GetExchangeRatesFromStringList(codes);
            if (exchangeRates.ToList().Count == 0)
            {
                logger.LogWarning("No exchange rates found for the provided currency codes: {CurrencyCodes}", currencyCodes);
                return BadRequest("No exchange rates found for the provided currency codes.");
            }

            return Ok(exchangeRates);
            
        }
    }
}
