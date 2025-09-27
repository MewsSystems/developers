using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Extensions;

namespace ExchangeRateUpdater.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRatesController> _logger;

    public ExchangeRatesController(IExchangeRateService exchangeRateService, ILogger<ExchangeRatesController> logger)
    {
        _exchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get exchange rates for specified currencies
    /// </summary>
    /// <param name="currencies">Comma-separated list of currency codes (e.g., USD,EUR,JPY)</param>
    /// <param name="date">Optional date in YYYY-MM-DD format. Defaults to today.</param>
    /// <returns>Exchange rates for the specified currencies</returns>
    /// <response code="200">Returns the exchange rates</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If no exchange rates found for the specified currencies</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ExchangeRateResponse>>> GetExchangeRates(
        [FromQuery] string currencies,
        [FromQuery] string? date = null)
    {
        var parsedDate = ValidateAndParseDate(date);
        var currencyObjects = ValidateAndrParseCurrencies(currencies);

        var exchangeRates = await _exchangeRateService.GetExchangeRates(currencyObjects, parsedDate.AsMaybe());
        if (!exchangeRates.Any())
        {
            var currencyList = string.Join(", ", currencyObjects.Select(c => c.Code));
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "No results found",
                Errors = new List<string> { $"No exchange rates found for the specified currencies: {currencyList}" }
            });
        }

        return Ok(new ApiResponse<ExchangeRateResponse>
        {
            Data = exchangeRates.ToExchangeRateResponse(parsedDate ?? DateTime.Today),
            Success = true,
            Message = "Exchange rates retrieved successfully"
        });
    }

    private static DateTime? ValidateAndParseDate(string? date)
    {
        if (string.IsNullOrEmpty(date))
        {
            return null;
        }

        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var validDate))
        {
            throw new ArgumentException($"Invalid date format. Expected format: YYYY-MM-DD (e.g., 2024-01-15). Received: '{date}'");
        }

        return validDate;
    }

    private static IEnumerable<Currency> ValidateAndrParseCurrencies(string currencies)
    {
        var currencyCodes = currencies.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(code => code.Trim().ToUpperInvariant())
            .ToHashSet();

        if (!currencyCodes.Any())
        {
            throw new ArgumentException("At least one currency code must be provided");
        }

        return currencyCodes.Select(code => new Currency(code));
    }
}
