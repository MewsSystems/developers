using ExchangeRateUpdater.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRatesController(
    ILogger<ExchangeRatesController> logger,
    IExchangeRateProvider exchangeRateProvider) : ControllerBase
{
    private readonly ILogger<ExchangeRatesController> _logger = logger;

    [HttpGet("{code}")]
    public ActionResult<ExchangeRate> Get([FromRoute] string code)
    {
        var exchangeRate = exchangeRateProvider.GetExchangeRates([new Currency(code)]).ToBlockingEnumerable()
            .FirstOrDefault();

        if (exchangeRate != null) return exchangeRate;

        return NotFound();
    }

    [HttpGet]
    public ActionResult<IEnumerable<ExchangeRate>> Get([FromQuery] IEnumerable<string> codes)
    {
        var rates = codes.Any()
                        ? exchangeRateProvider.GetExchangeRates(codes.Select(s => new Currency(s)))
                        : exchangeRateProvider.GetAllExchangeRates();

        return rates.ToBlockingEnumerable().ToList();
    }
}