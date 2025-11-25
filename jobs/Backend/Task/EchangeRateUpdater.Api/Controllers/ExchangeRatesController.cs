namespace ExchangeRateUpdater.Api.Controllers;

using Application.ExchangeRates.Dtos;
using Application.ExchangeRates.Query.GetExchangeRatesDaily;
using Mediator;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("exchange-rates")]
public class ExchangeRatesController (IMediator mediator) : Controller
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ExchangeRateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<ExchangeRateDto>>> GetExchangeRatesByDate(
        [FromQuery] GetExchangesRatesByDateQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}