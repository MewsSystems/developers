using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Controllers;

[ApiController]
[Route("api/v1/exchange-rates")]
[Produces("application/json")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IValidator<string[]> _currencyCodeValidator;
    private readonly ILogger<ExchangeRatesController> _logger;

    public ExchangeRatesController(IExchangeRateProvider exchangeRateProvider, IValidator<string[]> currencyCodeValidator, ILogger<ExchangeRatesController> logger)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _currencyCodeValidator = currencyCodeValidator;
        _logger = logger;
    }
    
    ///<summary>
    /// Fetches the desired currencies and returns the <see cref="ExchangeRate"/> based on the Source Currency for the current business day.
    /// </summary>>
    /// <param name="currencies" example="{'USD', 'EUR', CZK'}">Desired Currencies</param>
    [HttpGet("daily")]
    [ProducesResponseType(typeof(ActionResult<List<ExchangeRate>>),(int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ActionResult<string>),(int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> GetSelectedCurrencyCodesDaily([FromQuery] string[] currencies = null)
    {
        var validationResult = await _currencyCodeValidator.ValidateAsync(currencies);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            _logger.LogError("ExchangeRatesController_ValidationFailure {Errors}", errors);
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var result = await _exchangeRateProvider.GetExchangeRates(currencies);

        if (!result.Success)
        {
            _logger.LogError("ExchangeRatesController_ProviderError {Error}", result.Error);
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
}