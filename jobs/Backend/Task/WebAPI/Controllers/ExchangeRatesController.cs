using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeRatesController> _logger;


        public ExchangeRatesController(IExchangeRateService exchangeRateService, ILogger<ExchangeRatesController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets exchange rates for requested currencies.
        /// </summary>
        /// <param name="currencies">Comma-separated ISO 4217 currency codes (e.g., USD,EUR,CZK)</param>
        /// <returns>List of exchange rates</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Get([FromQuery] string currencies)
        {
            if (string.IsNullOrWhiteSpace(currencies)){
                _logger.LogWarning("Invalid request: missing 'currencies' parameter.");
                return BadRequest("Query parameter 'currencies' is required.");
            }
            
            var codes = currencies.Split(',').Select(c => c.Trim().ToUpper()).Where(c => c.Length == 3).Distinct();
            if (codes.Count() == 0){
                _logger.LogWarning("Invalid request: no valid currency codes provided. Raw input: {Input}", currencies);
                return BadRequest("No valid currency codes provided.");
            }
            var currencyObjs = codes.Select(code => new Currency(code));
            var rates = await _exchangeRateService.GetRatesAsync(currencyObjs);
            var result = rates.Select(r => new {
                source = r.Source.Code,
                target = r.Target.Code,
                value = r.Value
            });
            _logger.LogInformation("Returned {Count} exchange rates for request: {Currencies}", result.Count(), currencies);
            return Ok(result);
        }
    }
}
