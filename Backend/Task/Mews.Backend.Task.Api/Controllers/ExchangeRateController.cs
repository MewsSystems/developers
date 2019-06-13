using Mews.Backend.Task.Core;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Mews.Backend.Task.Api.Controllers
{
    [ApiController]
    [Route("api/exchange-rate")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async System.Threading.Tasks.Task<IActionResult> Get([FromQuery] string[] currencies)
        {
            if (currencies.Length == 0)
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "At least one currency is required."
                });

            var selectedCurrencies = currencies.Select(x => new Currency(x));
            var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(selectedCurrencies);

            return Ok(exchangeRates);
        }
    }
}
