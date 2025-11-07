using Mews.ExchangeRateUpdater.Application.ExchangeRates;
using Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Mews.ExchangeRateUpdater.Api.Controllers;

/// <summary>
/// Exchange rates controller.
/// </summary>
[Route("api/exchange-rates")]
[ApiController]
public class ExchangeRatesController: ControllerBase
{
    private readonly IExchangeRateAppService _exchangeRateAppService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="exchangeRateAppService"><see cref="IExchangeRateAppService"/></param>
    public ExchangeRatesController(IExchangeRateAppService exchangeRateAppService)
    {
        _exchangeRateAppService = exchangeRateAppService;
    }

    /// <summary>
    /// Returns an Allow HTTP header with the allowed HTTP methods.
    /// </summary>
    /// <returns>A 200 OK response.</returns>
    [HttpOptions]
    public IActionResult Options()
    {
        HttpContext.Response.Headers.AppendCommaSeparatedValues(
            HeaderNames.Allow,
            HttpMethods.Get,
            HttpMethods.Options);
        return Ok();
    }

    /// <summary>
    /// Gets all the exchange rates for today's date.
    /// </summary>
    /// <returns>A 200 OK response containing the exchange rates for today's date.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> GetExchangeRates()
    {
        return (await _exchangeRateAppService.GetTodayExchangeRatesAsync()).ToList();
    }
}
