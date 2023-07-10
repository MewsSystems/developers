using ExchangeRateUpdater.Application.Abstractions;
using ExchangeRateUpdater.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdaterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateUpdaterController : ControllerBase
    {
        private readonly ILogger<ExchangeRateUpdaterController> _logger;
        private readonly IExchangeRateProvider _exchangeRateUpdater;

        public ExchangeRateUpdaterController(ILogger<ExchangeRateUpdaterController> logger, IExchangeRateProvider exchangeRateUpdater)
        {
            _logger = logger;
            _exchangeRateUpdater = exchangeRateUpdater;
        }

        [HttpGet]
        [Route(("CbnTodayRate"))]
        public async Task<IActionResult> Get([FromQuery] string? rate)
        {
            if (string.IsNullOrEmpty(rate))
            {
                return BadRequest("Request parameter is not well formed");
            }

            IEnumerable<Currency> currencies = new[]
            {
                new Currency(rate),
            };

            var rates = await _exchangeRateUpdater.GetExchangeRatesAsync(currencies, DateTime.UtcNow);

            if(!rates.Any())
            {
                return NotFound();
            }

            return Ok(rates);
        }
    }
}