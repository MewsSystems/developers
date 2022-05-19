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
