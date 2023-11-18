using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers
{
    [ApiController]
    [Route("rates")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateProvider _cnbExchangeRateProvider;
        public ExchangeRatesController(IExchangeRateProvider cnbExchangeRateProvider)
        {
            _cnbExchangeRateProvider = cnbExchangeRateProvider ?? throw new ArgumentNullException(nameof(cnbExchangeRateProvider));
        }

        [HttpPost("daily/cnb")]
        public async Task<IActionResult> GetCnbExchangeRate(DailyExchangeRatesRequest request, CancellationToken cancellationToken)
        {
            var rates = await _cnbExchangeRateProvider.GetExchangeRatesAsync(request.Currencies, cancellationToken);
            return Ok(rates);
        }
    }
}