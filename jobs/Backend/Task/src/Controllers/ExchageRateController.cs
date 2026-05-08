using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Controllers;

[ApiController]
[Route("ExchangeRate")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateProvider _provider;

    public ExchangeRateController(IExchangeRateProvider provider)
    {
        _provider = provider;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExchangeRateResponse>>> GetExchangeRates(
        [FromQuery] string currencies, [FromQuery] string baseCurrency = "CZK")
    {
        if (string.IsNullOrWhiteSpace(currencies))
        {
            return BadRequest("Currency codes cannot be empty.");
        }
        
        var currencyObjects = currencies.Split(',')
            .Select(c => new Currency(c.Trim().ToUpper())).ToList();

        try
        {
            var exchangeRates = await _provider.GetExchangeRates(currencyObjects, new Currency(baseCurrency));

            return Ok(ExchangeRate.GetResponse(exchangeRates));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ExchangeRateApiException ex)
        {
            return StatusCode(503, $"Service Unavailable: {ex.Message}");
        }
    }
}