using Asp.Versioning;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/rates")]
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
            var currencies = request.Currencies.Select(curr => new Currency(curr));
            var rates = await _cnbExchangeRateProvider.GetExchangeRatesAsync(currencies, cancellationToken);
            return Ok(rates);
        }
    }
}