using ExchangeRates.Application.Providers;
using ExchangeRates.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
    Summary = "Retrieves daily exchange rates",
    Description = "Returns the latest exchange rates against the Czech koruna (CZK)."
    )]
    [ProducesResponseType(typeof(IEnumerable<ExchangeRate>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ExchangeRate>>> Get([FromQuery] string[]? currencies, CancellationToken cancellationToken)
    {
        var rates = await _exchangeRatesProvider.GetExchangeRatesAsync(currencies, cancellationToken);

        if (!rates.Any())
        {
            _logger.LogWarning("No exchange rates found for {Currencies}", currencies ?? Array.Empty<string>());
            return NotFound("No exchange rates found.");
        }

        return Ok(rates);
    }
}
