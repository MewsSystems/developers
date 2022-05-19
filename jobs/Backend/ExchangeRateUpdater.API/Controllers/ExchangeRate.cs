using ExchangeRateUpdater.RatesReader;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRate : ControllerBase
    {
        private readonly IAllCurrentRatesReaderService ratesReaderService;

        public ExchangeRate(IAllCurrentRatesReaderService ratesReaderService)
        {
            this.ratesReaderService = ratesReaderService;
        }

        /// <summary>
        /// Get all exchange rates
        /// </summary>
        /// <returns></returns>
        /// <response code = "200">Exchange rate returned</response>
        /// <response code = "400">Exchange rate not found</response>
        /// <response code = "500">Errors when trying to retirev the exchange rates</response>
        [HttpGet]
        public async Task<IActionResult> GetExchangeRate()
        {
            var ratesResult = await ratesReaderService.GetAllExchangeRates();
            if (ratesResult.Succsess)
            {
                if (!ratesResult.Value.Any())
                {
                    return new NotFoundResult();
                }
                return Ok(ratesResult.Value.Select(r => r.ToString()));
            }
            var failureText = ratesResult.FailureResons.Aggregate((partialErrorText, failure) => $"{partialErrorText} {failure} {Environment.NewLine}");
            return Problem(failureText);
        }
    }
}
