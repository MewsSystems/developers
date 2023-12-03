using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Host.WebApi.Dtos;
using ExchangeRateUpdater.Host.WebApi.Mappers;
using ExchangeRateUpdater.Host.WebApi.Dtos.Request;
using ExchangeRateUpdater.Domain.UseCases;

namespace ExchangeRateUpdater.Host.WebApi.Controllers
{
    [ApiController]
    [Route("/api/exchangeRates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProviderRepository _exchangeRateUpdaterRepository;
        private readonly ExchangeUseCase _exchangeUseCase;

        public ExchangeRateController(IExchangeRateProviderRepository exchangeRateUpdaterRepository, ExchangeUseCase exchangeUseCase)
        {
            _exchangeRateUpdaterRepository = exchangeRateUpdaterRepository ?? throw new ArgumentNullException(nameof(exchangeRateUpdaterRepository));
            _exchangeUseCase = exchangeUseCase ?? throw new ArgumentNullException(nameof(exchangeUseCase));
        }

       

        [HttpGet("defaultRates")]
        public async Task<IActionResult> GetDefaultUnitRatesAsync()
        {
            var defaultCZKRates = await _exchangeRateUpdaterRepository.GetDefaultUnitRates(DateTime.Now);

            return Ok(defaultCZKRates.Select(ExchangeRateMapper.ToDto));
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> BuyAsync([FromBody] ExchangeOrderDto exchangeOrderDto)
        {
            if (string.IsNullOrWhiteSpace(exchangeOrderDto.SourceCurrency)) return BadRequest("Source Currency has to be specified.");
            if (string.IsNullOrWhiteSpace(exchangeOrderDto.TargetCurrency)) return BadRequest("Target Currency has to be specified.");
            if (exchangeOrderDto.SumToExchange.HasValue == false) return BadRequest("SumToExchange has to be specified.");

            var exchangeOrder = exchangeOrderDto.ToOrderBuy();

            var exchangeResult = await _exchangeUseCase.ExecuteAsync(exchangeOrder);

            if (exchangeResult == null) return NotFound("We do not support exchange rates for the mentioned source/target currencies.");

            return Ok(exchangeResult.ToBuyResultDto());
        }


    }
}
