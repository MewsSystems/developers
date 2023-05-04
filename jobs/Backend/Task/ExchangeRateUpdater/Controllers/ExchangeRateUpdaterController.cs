namespace ExchangeRateUpdater.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRateUpdaterController : ControllerBase
{
    private readonly IExchangeRateProviderService _exchangeRateProviderService;
    private readonly ILogger<ExchangeRateUpdaterController> _logger;

    public ExchangeRateUpdaterController(
        IExchangeRateProviderService exchangeRateProviderService, 
        ILogger<ExchangeRateUpdaterController> logger)
    {
        _exchangeRateProviderService = exchangeRateProviderService;
        _logger = logger;
    }

    /// <summary>
    /// Get the exchange rates.
    /// </summary>
    /// <param name="request">Exchange rates request.</param>
    /// <returns>Returns the exchange rates.</returns>
    [HttpPost]
    public async Task<ActionResult<IEnumerable<string>>> GetExchangeRates([FromBody] ExchangeRateRequest request)
    {
        _logger.LogInformation($"Getting the exchange rates for currencies: {string.Join(", ", request.Currencies)}");
        var response = await _exchangeRateProviderService.GetExchangeRates(request);

        if (!response.Result)
            return BadRequest(response.Errors);

        return Ok(response.ExchangeRates.Select(x => x.ToString()));
    }
}
