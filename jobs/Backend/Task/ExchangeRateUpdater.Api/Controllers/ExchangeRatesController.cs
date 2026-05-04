using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRatesController(
    IExchangeRatesService exchangeRatesService,
    ILogger<ExchangeRatesController> logger) : ControllerBase
{
    /// <summary>
    /// Gets exchange rates for the specified currency codes.
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExchangeRate>>> Get([FromQuery] string codes)
    {
        logger.LogInformation("Requested Get Exchange rates for: {Codes}", codes);
        
        var currencyCodes = codes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var rates = await exchangeRatesService.GetRates(currencyCodes.ToList());
        return Ok(rates);
    }
}
