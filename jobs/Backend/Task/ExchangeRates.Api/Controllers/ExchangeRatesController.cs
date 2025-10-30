using Microsoft.AspNetCore.Mvc;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Application.Providers;

namespace ExchangeRates.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRatesProvider _exchangeRatesService;

        public ExchangeRatesController(ExchangeRatesProvider exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService;
        }

        /// <summary>
        /// Returns exchange rates for the specified currencies.
        /// </summary>
        /// <param name="currencies">Optional list of currency codes (e.g., USD, EUR). If empty, defaults will be used.</param>
        [HttpGet]
        public async Task<IActionResult> GetExchangeRates([FromQuery] string[]? currencies)
        {
            var currencyList = (currencies != null && currencies.Any())
                ? currencies.Select(c => new Currency(c)).ToArray()
                : new[]
                {
                    new Currency("USD"),
                    new Currency("EUR"),
                    new Currency("CZK"),
                    new Currency("JPY"),
                    new Currency("KES"),
                    new Currency("RUB"),
                    new Currency("THB"),
                    new Currency("TRY"),
                    new Currency("XYZ")
                };

            var rates = await _exchangeRatesService.GetExchangeRatesAsync(currencyList);

            return Ok(rates);
        }
    }
}
