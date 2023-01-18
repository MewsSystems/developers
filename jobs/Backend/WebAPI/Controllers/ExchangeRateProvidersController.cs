using Business.Abstract;
using Entities.Dtos;
using ExchangeRateUpdater;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateProvidersController : ControllerBase
    {
        private IExchangeRateProviderService _exchangeRateProviderService;

        public ExchangeRateProvidersController(IExchangeRateProviderService exchangeRateProviderService)
        {
            _exchangeRateProviderService= exchangeRateProviderService;
        }

        [HttpPost]
        public IActionResult ExchangeRates([FromBody] IEnumerable<Currency> currencies)
        {
            ConcurrencListRecord concurrencListDto = new ConcurrencListRecord(currencies);

            var result=_exchangeRateProviderService.GetExchangeRates(concurrencListDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

    }
}
