using AutoMapper;
using ExchangeRateUpdater.Abstractions.Contracts;
using ExchangeRateUpdater.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class RatesController : ControllerBase
{
    private readonly IExchangeRateProvider _provider;
    private readonly IMapper _mapper;

    public RatesController( 
        IExchangeRateProvider provider,
        IMapper mapper)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <summary>Gets CZK-based exchange rates for the given currency codes.</summary>
    /// <param name="currencies"> Currency codes.</param>
    /// <remarks>
    /// Returns only rates defined by CNB (CZK as base). If a code is unknown or unsupported, it’s ignored.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExchangeRateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public ActionResult<IEnumerable<ExchangeRateResponse>> GetExchangeRates(
        [FromQuery] IEnumerable<string> currencies)
    {
        if(currencies == null || !currencies.Any())
        {
            return BadRequest("At least one currency code must be provided.");
        }

        var requestCurrencies = currencies
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => new Currency(s.Trim().ToUpperInvariant()))
            .ToList();

        var exchangeRates = _provider.GetExchangeRates(requestCurrencies);
        var dto = _mapper.Map<IEnumerable<ExchangeRateResponse>>(exchangeRates);
        return Ok(dto);
    }
}
