using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExchangeRateUpdaterController : Controller
    {
        IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateUpdaterController(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        [HttpPost("GetExchangeRates")]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates([FromBody] IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRate> exchangeRates = Enumerable.Empty<ExchangeRate>();

            try
            {
                exchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(exchangeRates);
        }
    }
}
