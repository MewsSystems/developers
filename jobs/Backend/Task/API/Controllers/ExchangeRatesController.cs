using Api.Controllers;
using Application.Extensions;
using Application.UseCases.ExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/{language}/")]
public class ExchangeRatesController(ISender sender) : ControllerWithMediatR(sender)
{
    [HttpGet]
    [Route("exchangeRates")]
    public async Task<IActionResult> GetDailyAsync(
        [FromRoute] string language, 
        [FromQuery] string currencyCode, 
        CancellationToken token)
    {
        var result = await Send(new GetDailyExchangeRateRequest().GetQuery(currencyCode, language), token);
        return result.IsSuccess ? Ok(result.Value) : result.GetErrorResponse();
    }

    [HttpGet]
    [Route("exchangeRate")]
    public async Task<IActionResult> GetExchangeRateAsync(
        [FromRoute] string language,
        [FromQuery] string fromCurrency, 
        [FromQuery] string toCurrency, 
        CancellationToken token)
    {
        var result = await Send(new GetExchangeRateRequest().GetQuery(language, fromCurrency, toCurrency), token);
        return result.IsSuccess ? Ok(result.Value) : result.GetErrorResponse();
    }
}
