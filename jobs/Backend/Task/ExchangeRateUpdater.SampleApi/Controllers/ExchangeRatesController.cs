using ExchangeRateUpdater.Providers;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.SampleApi.Controllers
{
    [ApiController]
    [Route("v1/exchangeRates")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateProvider provider;

        public ExchangeRatesController(IExchangeRateProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return this.Ok(await this.provider.GetExchangeRates());
        }
    }
}