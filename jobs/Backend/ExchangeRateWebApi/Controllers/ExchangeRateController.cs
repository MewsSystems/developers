using ExchangeRateUpdater;
using ExchangeRateWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRate exchangeRate;

        public ExchangeRateController(IExchangeRate exchangeRate)
        {
            this.exchangeRate = exchangeRate;
        }

        [HttpGet]
        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            return exchangeRate.MapDataToExchangeRates();
        }
    }

}