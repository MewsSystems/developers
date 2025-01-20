using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Controllers;

[ApiController]
[Route("/api/exchangerates")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRatesController> _logger;


    public ExchangeRatesController(IExchangeRateService exchangeRateService, ILogger<ExchangeRatesController> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }
    /// <summary>
    /// Gets the exchange rates for the currencies requested and the specified date
    /// Uses POST to get an objetct (containing a list) as parameter
    /// </summary>
    /// <param name="exchangeRatesRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("getExchangeRates")]
    public async Task<ActionResult> GetDailyExchangeRatesAsync([FromBody] ExchangeRateRequestDto exchangeRatesRequest, CancellationToken cancellationToken)
    {
        if (exchangeRatesRequest.ExchangeRatesDetails is null || !exchangeRatesRequest.ExchangeRatesDetails.Any())
        {
            _logger.LogError("Exchange rate(s) cannot be empty or null");
            return BadRequest("Exchange rate(s) cannot be empty or null");
        }            

        var result = await _exchangeRateService.GetExchangeRates(exchangeRatesRequest.ExchangeRatesDetails, exchangeRatesRequest.Date, cancellationToken);

        _logger.LogInformation("Exchange rates successfully obtained.");
        return Ok(result);
    }

}
