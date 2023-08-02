using Application.Providers;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRatesController : ControllerBase
{
    private readonly ILogger<ExchangeRatesController> _logger;
    private readonly ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRatesController(ILogger<ExchangeRatesController> logger, ExchangeRateProvider exchangeRateProvider)
    {
        _logger = logger;
        _exchangeRateProvider = exchangeRateProvider;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ExchangeRatesDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLatestRates()
    {
        // TODO Move error handling logic to a common place (e.g. custom exception handling middleware)
        try
        {
            var rates = await _exchangeRateProvider.GetExchangeRatesAsync();
            return Ok(rates.Select(r => new ExchangeRatesDto(
                r.Amount,
                r.CurrencyCode,
                r.Rate,
                r.ValidFor)));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}