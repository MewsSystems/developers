using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace ExchangeRateUpdaterApi.Controllers;

[ApiController]
[Route("/api/exchangerates")]
public class ExchangeRatesController
{
    private readonly Logger _logger;
    
    public ExchangeRatesController(Logger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("getExchangeRates")]
    public async Task<ActionResult> GetDailyExchangeRatesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}