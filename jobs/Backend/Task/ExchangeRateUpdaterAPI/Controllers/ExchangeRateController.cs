using ExchangeRateUpdater;
using ExchangeRateUpdaterAPI.Services.ExchangeRateProviderService;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdaterAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                IEnumerable<ExchangeRate> exchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies);
                return Ok(exchangeRates);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest("Invalid format of exchange rate data. " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. " + ex.Message);
            }
        }
    }
}

