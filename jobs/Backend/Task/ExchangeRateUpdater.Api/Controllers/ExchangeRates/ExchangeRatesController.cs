using ExchangeRateUpdater.Application.GetExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers.ExchangeRates;

[ApiController]
[Route("[controller]")]
public class ExchangeRatesController : ControllerBase
{
    private readonly ISender _sender;
    
    public ExchangeRatesController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IActionResult> GetExchangeRates([FromBody] GetExchangeRatesQuery request)
    {
        var response = await _sender.Send(request);
        return Ok(response);
    }
}