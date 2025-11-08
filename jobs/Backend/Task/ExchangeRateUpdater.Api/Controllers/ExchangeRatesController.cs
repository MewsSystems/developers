using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Extensions;
using ExchangeRateUpdater.Api.Binders;

namespace ExchangeRateUpdater.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRatesController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
    }

    /// <summary>
    /// Get exchange rates for specified currencies on specified date or closest business day if date is not provided.
    /// </summary>
    /// <param name="currencies">Comma-separated list of currency codes provided as a list of strings like [USD,EUR,JPY] or multiple currency parameters</param>
    /// <param name="date">Optional date in YYYY-MM-DD format. Defaults to today if not present or if a future date is provided.</param>
    /// <returns>Exchange rates for the specified currencies</returns>
    /// <response code="200">Returns the exchange rates</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If no exchange rates found for the specified currencies</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ExchangeRateResponseDto>>> GetExchangeRates(
        [ModelBinder(BinderType = typeof(CommaSeparatedQueryBinder))] List<string> currencies,
        [FromQuery] DateOnly? date = null)
    {
        var currencyObjects = ParseCurrencies(currencies);

        var exchangeRates = await _exchangeRateService.GetExchangeRates(currencyObjects, date.AsMaybe());
        if (!exchangeRates.Any())
        {
            var currencyList = string.Join(", ", currencyObjects.Select(c => c.Code));
            return NotFound(ApiResponseBuilder.NotFound("No results found",
                $"No exchange rates found for the specified currencies: {currencyList}"));
        }

        return Ok(ApiResponseBuilder.Success(
            exchangeRates.ToExchangeRateResponse(date.AsMaybe()),
            "Exchange rates retrieved successfully"));
    }

    private static IEnumerable<Currency> ParseCurrencies(List<string> currencies)
    {
        var currencyCodes = currencies.Select(code => code.Trim().ToUpperInvariant()).ToHashSet();

        if (!currencyCodes.Any())
        {
            throw new ArgumentException("At least one currency code must be provided");
        }

        return currencyCodes.Select(code => new Currency(code));
    }
}
