using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Controllers;

[ApiController]
[Route("ExchangeRate")]
public class ExchageRateController : ControllerBase
{
    private readonly IExchangeRateProvider _provider;

    public ExchageRateController(IExchangeRateProvider provider)
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

        if (string.IsNullOrWhiteSpace(baseCurrency))
        {
            return BadRequest("Base currency cannot be empty.");
        }

        var currencyObjects = currencies.Split(',')
            .Select(c => new Currency(c.Trim().ToUpper())).ToList();

        try
        {
            var exchangeRates = await _provider.GetExchangeRates(currencyObjects, new Currency(baseCurrency));

            return Ok(exchangeRates.Select(er => new ExchangeRateResponse
            {
                SourceCurrency = er.SourceCurrency.Code,
                TargetCurrency = er.TargetCurrency.Code,
                ExchangeRate = er.Value
            }));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ExternalExchangeRateApiException ex)
        {
            // Log the exception if logging is configured
            return StatusCode(503, $"Service Unavailable: {ex.Message}");
        }
    }
}