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
        private readonly BuyOrderUseCase _buyOrderUseCase;

        public ExchangeRateController(IExchangeRateProviderRepository exchangeRateUpdaterRepository, BuyOrderUseCase buyOrderUseCase)
        {
            _exchangeRateUpdaterRepository = exchangeRateUpdaterRepository ?? throw new ArgumentNullException(nameof(exchangeRateUpdaterRepository));
            _buyOrderUseCase = buyOrderUseCase ?? throw new ArgumentNullException(nameof(buyOrderUseCase));
        }

       

        [HttpGet("defaultRates")]
        public async Task<IActionResult> GetDefaultUnitRatesAsync()
        {
            var defaultCZKRates = await _exchangeRateUpdaterRepository.GetDefaultUnitRates();

            return Ok(defaultCZKRates.Select(ExchangeRateMapper.ToDto));
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> BuyAsync([FromBody] BuyOrderDto orderBuyDto)
        {
            if (string.IsNullOrWhiteSpace(orderBuyDto.SourceCurrency)) return BadRequest("Source Currency has to be specified.");
            if (string.IsNullOrWhiteSpace(orderBuyDto.TargetCurrency)) return BadRequest("Target Currency has to be specified.");
            if (orderBuyDto.SumToExchange.HasValue == false) return BadRequest("SumToExchange has to be specified.");

            var orderBuy = orderBuyDto.ToOrderBuy();

            var buyResult = await _buyOrderUseCase.ExecuteAsync(orderBuy);

            if (buyResult == null) return NotFound("We do not support exchange rates for the mentioned source/target currencies.");

            return Ok(buyResult.ToBuyResultDto());
        }


    }
}
