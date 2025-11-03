using ExchangeRates.Api.DTOs;
using ExchangeRates.Application.Providers;
using ExchangeRates.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRatesProvider _exchangeRatesProvider;
    private readonly ILogger<ExchangeRatesController> _logger;

    public ExchangeRatesController(IExchangeRatesProvider exchangeRatesProvider, ILogger<ExchangeRatesController> logger)
    {
        _exchangeRatesProvider = exchangeRatesProvider;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExchangeRate>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ExchangeRate>>> Get([FromQuery] GetExchangeRatesRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currencies = request.Currencies ?? Array.Empty<string>();
        var rates = await _exchangeRatesProvider.GetExchangeRatesAsync(currencies, cancellationToken);
        return Ok(rates);
    }
}
