using ExchangeRateUpdater.WebApi.Services.ExchangeRateProvider;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRateProviderController : Controller
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IValidator<IEnumerable<Currency>> _validator;

    public ExchangeRateProviderController(IExchangeRateProvider exchangeRateProvider, IValidator<IEnumerable<Currency>> validator)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _validator = validator;
    }

    /// <summary>
    /// Returns all available exchange rates for a given list of currencies with source currency: CZK.
    /// </summary>
    /// <param name="currencies">List of currencies</param>
    /// <returns>Available exchange rates with CZK as base currency.</returns>
    /// <response code="200">Returns a list with the available exchange rates</response>
    /// <response code="500">Bad configuration</response>
    /// <response code="503">Exchange rates source is unavailable</response>
    [HttpPost("ExchangeRates")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<ExchangeRate>>>> ExchangeRates([FromBody]IEnumerable<Currency> currencies)
    {
        var validationResult = await _validator.ValidateAsync(currencies);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ServiceResponse<IEnumerable<ExchangeRate>>
            {
                Data = new ExchangeRate[] { },
                Success = false,
                Message = "Invalid input model"
            });
        }

        var serviceResponse = await _exchangeRateProvider.GetExchangeRates(currencies);
    
        if (!serviceResponse.Success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, serviceResponse);
        }

        return Ok(serviceResponse);
    }
}