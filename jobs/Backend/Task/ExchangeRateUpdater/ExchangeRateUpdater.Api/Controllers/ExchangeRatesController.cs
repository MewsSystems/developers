using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Application.Interfaces;

namespace ExchangeRateUpdater.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateService _service;
    private readonly ILogger<ExchangeRatesController> _logger;

    public ExchangeRatesController(IExchangeRateService service, ILogger<ExchangeRatesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct)
    {
        _logger.LogInformation("GET /api/exchangerates");
        var result = await _service.GetAllRatesAsync(ct);
        return Ok(result);
    }

    [HttpGet("{currencyCode}")]
    public async Task<ActionResult> Get(string currencyCode, CancellationToken ct)
    {
        _logger.LogInformation("GET /api/exchangerates/{Currency}", currencyCode);
        var result = await _service.GetRateByCurrencyAsync(currencyCode, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
