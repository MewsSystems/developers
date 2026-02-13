using ExchangeRateAPI.Adapters;
using ExchangeRateAPI.Models;
using ExchangeRateUpdater.Business;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider __ExchangeRateProvider;

        private readonly List<string> __Currencies =
        [
            "AUD",
            "USD",
            "EUR",
            "CZK",
            "JPY",
            "KES",
            "RUB",
            "THB",
            "TRY",
            "XYZ"
        ];

        public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
        {
            __ExchangeRateProvider = exchangeRateProvider;
        }

        [HttpGet(Name = "GetExchangeRates")]
        public async Task<List<ExchangeRateResponse>> GetAsync()
        {
            List<ExchangeRateResponse> _Response = (await __ExchangeRateProvider.GetExchangeRatesAsync(ExchangeRateAdapter.ConvertToCurrencyList(__Currencies))).ToExchangeRateResponses();
            return _Response;
        }
    }
}
