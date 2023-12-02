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

        
    }
}
