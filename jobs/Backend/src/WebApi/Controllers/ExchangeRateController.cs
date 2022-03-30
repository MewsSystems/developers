using ExchangeRate.Infrastructure.CNB.Core;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetCNBExchangeRates() => Ok(await _exchangeRateProvider.GetExchangeRates());
}
