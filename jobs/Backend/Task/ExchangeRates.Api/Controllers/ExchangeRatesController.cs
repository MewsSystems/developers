using ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRates.Contracts.ExchangeRates;
using ExchangeRates.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers;

[ApiController]
[Route("exchange-rates")]
public class ExchangeRatesController : ControllerBase
{
    private readonly ISender _mediator;

    public ExchangeRatesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(DateTime? day)
    {
        var query = new GetExchangeRatesQuery(day);

        var result = await _mediator.Send(query);

        return Ok(Map(result));
    }

    private static IEnumerable<ExchangeRateResponse> Map(IEnumerable<ExchangeRate> exchangeRates)
    {
        return exchangeRates.Select(x => new ExchangeRateResponse(
            x.SourceCurrency.Code,
            x.TargetCurrency.Code,
            x.Value));
    }
}