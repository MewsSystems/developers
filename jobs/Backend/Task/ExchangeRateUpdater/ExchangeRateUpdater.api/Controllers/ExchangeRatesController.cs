using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Controllers
{
    [ApiController]
    [Route("/api/exchangerates")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeRatesController> _logger;

        public ExchangeRatesController(ILogger<ExchangeRatesController> logger, IExchangeRateService exchangeRateService)
        {
            _logger = logger;
            _exchangeRateService = exchangeRateService;
        }
        /// <summary>
        /// Gets the exchange rates for the currencies requested and the specified date
        /// Uses POST to get an objetct (containing a list) as parameter
        /// </summary>
        /// <param name="exchangeRatesRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("getExchangeRates")]
        public async Task<ActionResult> GetDailyExchangeRatesAsync([FromBody] ExchangeRateRequestDto exchangeRatesRequest, CancellationToken cancellationToken)
        {
            if (!exchangeRatesRequest.ExchangeRatesDetails.Any())
                return BadRequest("Exchange rate(s) cannot be empty or null");

            var result = await _exchangeRateService.GetExchangeRates(exchangeRatesRequest.ExchangeRatesDetails, exchangeRatesRequest.Date, cancellationToken);

            return Ok(result);
        }

    }
}
