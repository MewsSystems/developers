using ExchangeRateUpdater.API.DTOs;
using ExchangeRateUpdater.Application.Clients;
using ExchangeRateUpdater.Application.ExchangeRates;
using ExchangeRateUpdater.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.API.Controllers;

public class ExchangeRateController : BaseApiController
{
    /// <summary>
    /// This is used here instead of creating now a seperate console application
    /// </summary>
    private readonly IExchangeRateProvider _exchangeRateProvider;

    private readonly ICzbExchangeRateClient _czbExchangeRateClient;

    public ExchangeRateController(IExchangeRateProvider exchangeRateProvider, ICzbExchangeRateClient czbExchangeRateClient)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _czbExchangeRateClient = czbExchangeRateClient;
    }

    /// <summary>
    ///  Get Exchange rates based on the given source
    /// </summary>
    /// <returns></returns>
    [HttpGet("sources")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSourceRates()
    {
        var rates = await _exchangeRateProvider.GetExchangeRates(currencies);

        return HandleResult(rates);
    }

    /// <summary>
    ///  Get specific rates
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetRates([FromQuery] RateDto rate)
    {
        var rates = await _czbExchangeRateClient.GetExchangeRate(rate.Currency.ToUpper(), rate.Date);

        return HandleResult(rates);
    }

    readonly IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };
}
