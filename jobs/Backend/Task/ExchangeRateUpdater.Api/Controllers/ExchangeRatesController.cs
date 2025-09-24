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
        try
        {
            var dateValidationResult = ValidateAndParseDate(date);
            if (dateValidationResult.HasError)
            {
                return BadRequest(dateValidationResult.ErrorResponse);
            }

            var currencyCodes = currencies.Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            if (!TryCreateCurrencyObjects(currencyCodes, out var currencyObjects))
            {
                return BadRequest(CreateErrorResponse("At least one currency code must be provided", "At least one currency code must be provided"));
            }

            var exchangeRates = await _exchangeRateService.GetExchangeRates(currencyObjects, dateValidationResult.ParsedDate.AsMaybe());

            if (!exchangeRates.Any())
            {
                var currencyList = string.Join(", ", currencyObjects.Select(c => c.Code));
                return NotFound(CreateErrorResponse(
                    $"No exchange rates found for the specified currencies: {currencyList}",
                    $"No exchange rates found for the specified currencies: {currencyList}"));
            }

            var response = exchangeRates.ToExchangeRateResponse(dateValidationResult.ParsedDate ?? DateTime.Today);

            return Ok(new ApiResponse<ExchangeRateResponse>
            {
                Data = response,
                Success = true,
                Message = "Exchange rates retrieved successfully"
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid currency code provided: {Message}", ex.Message);
            return BadRequest(CreateErrorResponse($"Invalid currency code: {ex.Message}", $"Invalid currency code: {ex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching exchange rates");
            return StatusCode(StatusCodes.Status500InternalServerError, CreateErrorResponse("An error occurred while processing your request", "An error occurred while processing your request"));
        }
    }

    private static bool TryCreateCurrencyObjects(HashSet<string> currencyCodes, out IEnumerable<Currency> currencies)
    {
        currencies = new List<Currency>();
        if (!currencyCodes.Any())
            return false;

        currencies = currencyCodes.Select(code => new Currency(code.Trim().ToUpperInvariant()));
        return true;
    }

    private static ApiResponse CreateErrorResponse(string message, string error)
    {
        return new ApiResponse
        {
            Message = message,
            Errors = new List<string> { error },
            Success = false
        };
    }

    private static (bool HasError, ApiResponse? ErrorResponse, DateTime? ParsedDate) ValidateAndParseDate(string? date)
    {
        if (string.IsNullOrEmpty(date))
            return (false, null, null);

        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var validDate))
        {
            var errorMessage = $"Invalid date format. Expected format: YYYY-MM-DD (e.g., 2024-01-15). Received: '{date}'";
            return (true, CreateErrorResponse(errorMessage, errorMessage), null);
        }

        return (false, null, validDate);
    }
}
