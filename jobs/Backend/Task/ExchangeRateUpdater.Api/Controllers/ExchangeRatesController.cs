using ExchangeRateUpdater.ApplicationServices.ExchangeRates;
using ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers;

/// <summary>
/// Exchange Rates Controller
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[Route("api/exchange-rates")]
[ApiController]
public class ExchangeRatesController: ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRatesController"/> class.
    /// </summary>
    /// <param name="exchangeRateAppService">The exchange rate application service.</param>
    public ExchangeRatesController(IExchangeRateService exchangeRateAppService)
    {
        _exchangeRateService = exchangeRateAppService;
    }

    /// <summary>
    /// Gets the exchange rates.
    /// </summary>
    /// <returns>A Http status code 200 response containing the exchange rates for today's date.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> GetExchangeRates()
    {
        return (await _exchangeRateService.GetTodayExchangeRatesAsync()).ToList();
    }
}
