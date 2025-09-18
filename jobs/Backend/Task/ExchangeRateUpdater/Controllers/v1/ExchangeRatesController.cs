using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Controllers.v1.Requests;
using ExchangeRateUpdater.Controllers.v1.Responses;
using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Controllers.v1;

[ApiController]
[Route("api/[controller]")]

public sealed class ExchangeRatesController(IExchangeRateProvider exchangeRateProvider) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(GetExchangeRatesForCurrenciesResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Route(nameof(GetExchangeRatesForCurrencies))]
    public async Task<IActionResult> GetExchangeRatesForCurrencies(GetExchangeRatesForCurrenciesRequest getExchangeRatesForCurrenciesRequest, CancellationToken cancellationToken)
    {
        try
        {
            var currencies = getExchangeRatesForCurrenciesRequest.Currencies.Select(x =>
                new Currency(x)).ToArray();
            
            var result = await exchangeRateProvider.GetExchangeRatesForCurrenciesAsync(currencies, cancellationToken);
            var rates = result as ExchangeRate[] ?? result.ToArray();
   
            var exchangeRatesForCurrenciesResponse = new GetExchangeRatesForCurrenciesResponse
            {
                ExchangeRates = rates.ToImmutableArray()
            };
            
            return Ok(exchangeRatesForCurrenciesResponse);
        }
        catch (UnknownCurrencyException unknownCurrencyException)
        {
            return BadRequest(unknownCurrencyException.Message);
        }
        catch (InvalidExchangeRateDataException invalidExchangeRateDataException)
        {
            return BadRequest(invalidExchangeRateDataException.Message);
        }
        catch (Exception exception)
        {
            return Problem(exception.Message);
        }
    }
}