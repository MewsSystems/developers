using Asp.Versioning;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/exchange_rates")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateProvider _cnbExchangeRateProvider;
        public ExchangeRatesController(IExchangeRateProvider cnbExchangeRateProvider)
        {
            _cnbExchangeRateProvider = cnbExchangeRateProvider ?? throw new ArgumentNullException(nameof(cnbExchangeRateProvider));
        }

        [HttpPost("daily/cnb")]
        [ProducesResponseType(typeof(List<ExchangeRate>), 200)]
        public async Task<IActionResult> GetCnbExchangeRate(DailyExchangeRatesRequest request, CancellationToken cancellationToken)
        {
            var currencies = request.CurrencyCodes.Select(curr => new Currency(curr));
            IEnumerable<ExchangeRate> rates = await _cnbExchangeRateProvider.GetExchangeRatesAsync(currencies, cancellationToken);
            return Ok(rates);
        }
    }
}