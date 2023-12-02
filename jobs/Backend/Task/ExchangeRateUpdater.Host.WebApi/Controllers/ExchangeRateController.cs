using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Host.WebApi.Dtos;

namespace ExchangeRateUpdater.Host.WebApi.Controllers
{
    [ApiController]
    [Route("/api/exchangeRate")]
    public class ExchangeRateController : ControllerBase
    {
        private IExchangeRateProviderRepository _exchangeRateUpdaterRepository;


        public ExchangeRateController(IExchangeRateProviderRepository exchangeRateUpdaterRepository)
        {
            _exchangeRateUpdaterRepository = exchangeRateUpdaterRepository ?? throw new ArgumentNullException(nameof(exchangeRateUpdaterRepository));
        }

       

        [HttpGet("defaultRates")]
        public async Task<IActionResult> GetDefaultUnitRatesAsync()
        {
            return Ok(await _exchangeRateUpdaterRepository.GetDefaultUnitRates());
        }

        [HttpPost("orders/buy")]
        public async Task<IActionResult> BuyAsync([FromBody] OrderBuyDto orderBuyDto)
        {
            if (string.IsNullOrWhiteSpace(orderBuyDto.SourceCurrency)) return BadRequest("Source Currency has to be specified");
            if (string.IsNullOrWhiteSpace(orderBuyDto.TargetCurrency)) return BadRequest("Target Currency has to be specified");
            if (orderBuyDto.SumToExchange <= 0.0m) return BadRequest("SumToExchange has to be a positive value.");



            return Ok();
        }
    }
}
