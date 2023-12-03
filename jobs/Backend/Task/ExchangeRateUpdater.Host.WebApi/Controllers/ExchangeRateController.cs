using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Host.WebApi.Dtos;
using ExchangeRateUpdater.Host.WebApi.Mappers;

namespace ExchangeRateUpdater.Host.WebApi.Controllers
{
    [ApiController]
    [Route("/api/exchangeRates")]
    public class ExchangeRateController : ControllerBase
    {
        private IExchangeRateProviderRepository _exchangeRateUpdaterRepository;


        public ExchangeRateController(IExchangeRateProviderRepository exchangeRateUpdaterRepository)
        {
            _exchangeRateUpdaterRepository = exchangeRateUpdaterRepository ?? throw new ArgumentNullException(nameof(exchangeRateUpdaterRepository));
        }

       

        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultUnitRatesAsync()
        {
            var defaultCZKRates = await _exchangeRateUpdaterRepository.GetDefaultUnitRates();

            return Ok(defaultCZKRates.Select(ExchangeRateMapper.ToDto));
        }

        
    }
}
