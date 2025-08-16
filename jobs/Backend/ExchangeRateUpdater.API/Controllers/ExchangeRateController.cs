using ExchangeRateUpdater.Infrastructure.BusinessLogic;
using ExchangeRateUpdater.Model.Common;
using ExchangeRateUpdater.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.API.Controllers
{
    [ApiController]
    [Route("updater-api/[controller]/[action]")]
    public class ExchangeRateController : Controller
    {
        private readonly IEnumerable<Currency> currencies = new[]
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

        private readonly IExchangeRateProvider _provider;

        public ExchangeRateController(IExchangeRateProvider provider)
        {
            _provider = provider;
        }

        [HttpPost]
        public async Task<ExchangeRateResponseDto> GetExchangeRatesAsync([FromBody] ExchangeRateRequestDto request)
        {
            request.Currencies = currencies;
            return await _provider.GetExchangeRatesAsync(request);
        }
    }
}
