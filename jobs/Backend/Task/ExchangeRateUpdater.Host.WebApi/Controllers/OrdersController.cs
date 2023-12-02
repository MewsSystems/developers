using ExchangeRateUpdater.Domain.UseCases;
using ExchangeRateUpdater.Host.WebApi.Dtos.Request;
using ExchangeRateUpdater.Host.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Host.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly BuyOrderUseCase _orderBuyUseCase;

    public OrdersController(BuyOrderUseCase orderBuyUseCase)
    {
        this._orderBuyUseCase = orderBuyUseCase ?? throw new ArgumentNullException(nameof(orderBuyUseCase));
    }


    [HttpPost("/buy")]
    public async Task<IActionResult> BuyAsync([FromBody] BuyOrderDto orderBuyDto)
    {
        if (string.IsNullOrWhiteSpace(orderBuyDto.SourceCurrency)) return BadRequest("Source Currency has to be specified.");
        if (string.IsNullOrWhiteSpace(orderBuyDto.TargetCurrency)) return BadRequest("Target Currency has to be specified.");
        if (orderBuyDto.SumToExchange.HasValue) return BadRequest("SumToExchange has to be specified.");

        var orderBuy = orderBuyDto.ToOrderBuy();

        var buyResult = await _orderBuyUseCase.ExecuteAsync(orderBuy);

        return Ok(buyResult?.ToBuyResultDto());
    }
}
