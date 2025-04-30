using ExchangeRateProviderAPI_PaolaRojas.Models.Handlers;
using ExchangeRateProviderAPI_PaolaRojas.Models.Requests;
using ExchangeRateProviderAPI_PaolaRojas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProviderAPI_PaolaRojas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
    public class ExchangeRatesController(IExchangeRateService exchangeRateService) : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;

        [HttpPost("rates")]
        public async Task<IActionResult> GetExchangeRates([FromBody] CurrencyRequest request)
        {
            var result = await _exchangeRateService.GetExchangeRatesAsync(request.Currencies);
            return result?.ExchangeRates != null && result.ExchangeRates.Any()
                ? Ok(result)
                : NotFound("No exchange rates found for the provided currencies.");
        }
    }
}