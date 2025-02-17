namespace ExchangeRateProvider.Host.WebApi.Controllers;

using Application.Interfaces;
using Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/exchangerates")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    /// <summary>
    ///     Retrieves exchange rates for the specified currencies.
    /// </summary>
    /// <param name="currencies">A list of currency codes in ISO 4217 format.</param>
    /// <returns>A response containing resolved exchange rates and unresolved currencies.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetExchangeRatesResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetExchangeRates([FromBody] GetExchangeRatesRequest currencies)
    {
        if (currencies == null || !currencies.Items.Any())
        {
            return BadRequest("Currencies list cannot be empty.");
        }

        var result = await _exchangeRateService.GetExchangeRatesAsync(currencies.Items);

        var response = new GetExchangeRatesResponse
            { Rates = result.Rates, CurrenciesNotResolved = result.CurrenciesNotResolved };

        return Ok(response);
    }
}
