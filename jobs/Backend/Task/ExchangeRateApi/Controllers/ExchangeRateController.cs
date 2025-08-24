using ExchangeRateApi.Models;
using ExchangeRateProviders;
using ExchangeRateProviders.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;

namespace ExchangeRateApi.Controllers;

[ApiController]
[SwaggerTag("Exchange Rate operations for retrieving currency exchange rates")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateProviderFactory _exchangeRateProviderFactory;
    private readonly ILogger<ExchangeRateController> _logger;
    private readonly IValidator<ExchangeRateRequest>? _requestValidator;
    private static readonly Regex QueryCodesRegex = new("^[A-Za-z]{3}(?:,[A-Za-z]{3})*$", RegexOptions.Compiled);

    public ExchangeRateController(
        IExchangeRateProviderFactory exchangeRateProviderFactory, 
        ILogger<ExchangeRateController> logger,
        IValidator<ExchangeRateRequest>? requestValidator = null)
    {
        _exchangeRateProviderFactory = exchangeRateProviderFactory;
        _logger = logger;
        _requestValidator = requestValidator;
    }

    /// <summary>
    /// Get exchange rates for specified currencies using JSON request
    /// </summary>
    /// <param name="request">The exchange rate request containing currency codes and optional target currency</param>
    /// <param name="cancellationToken">Request cancellation token</param>
    /// <returns>Exchange rates for the requested currencies</returns>
    /// <response code="200">Returns the exchange rates for the requested currencies</response>
    /// <response code="400">If the request is invalid or no currency codes are provided</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost(ApiEndpoints.ExchangeRates.GetAllByRequestBody)]
    [SwaggerOperation(
        Summary = "Get exchange rates using POST request",
        Description = "Retrieves exchange rates for specified currencies using a JSON request body. Supports multiple currencies and custom target currency.",
        OperationId = "GetExchangeRatesPost")]
    [SwaggerResponse(200, "Exchange rates retrieved successfully", typeof(ExchangeRateResponse))
    ]
    [SwaggerResponse(400, "Invalid request - missing or invalid currency codes")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ExchangeRateResponse>> GetExchangeRates(
        [FromBody, SwaggerRequestBody("Request containing currency codes and optional target currency")] ExchangeRateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Received request for exchange rates with {Count} currencies", request.CurrencyCodes.Count);

            if (request.CurrencyCodes == null || !request.CurrencyCodes.Any())
            {
                _logger.LogWarning("Exchange rate request received with empty currency codes");
                return BadRequest("At least one currency code must be provided");
            }

            // Validate request using FluentValidation if validator is configured
            if (_requestValidator != null)
            {
                ValidationResult validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                    _logger.LogWarning("Validation failed for request: {Errors}", string.Join(", ", errors));
                    return BadRequest(errors);
                }
            }

            // Use provided target currency or default to CZK
            var targetCurrency = request.TargetCurrency ?? "CZK";
            
            // Get the exchange rate provider for the target currency
            var provider = _exchangeRateProviderFactory.GetProvider(targetCurrency);
            
            // Convert currency codes to Currency objects
            var currencies = request.CurrencyCodes
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => new Currency(code.ToUpperInvariant()))
                .ToList();

            if (!currencies.Any())
            {
                _logger.LogWarning("No valid currency codes provided after filtering");
                return BadRequest("No valid currency codes provided");
            }

            // Get exchange rates
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies, cancellationToken);
            var ratesList = exchangeRates.ToList();

            _logger.LogInformation("Successfully retrieved {Count} exchange rates for target currency {TargetCurrency}", 
                ratesList.Count, targetCurrency);

            // Map to response model
            var response = new ExchangeRateResponse
            {
                TargetCurrency = targetCurrency,
                Rates = ratesList.Select(rate => new ExchangeRateDto
                {
                    SourceCurrency = rate.SourceCurrency.Code,
                    TargetCurrency = rate.TargetCurrency.Code,
                    Rate = rate.Value,
                    ValidFor = rate.ValidFor
                }).ToList()
            };

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation when getting exchange rates");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while getting exchange rates");
            return StatusCode(500, "An error occurred while retrieving exchange rates");
        }
    }

    /// <summary>
    /// Get exchange rates for specified currencies using query parameters
    /// </summary>
    /// <param name="currencies">Comma-separated list of currency codes (e.g., "USD,EUR,JPY")</param>
    /// <param name="targetCurrency">The target currency (defaults to "CZK")</param>
    /// <param name="cancellationToken">Request cancellation token</param>
    /// <returns>Exchange rates for the requested currencies</returns>
    /// <response code="200">Returns the exchange rates for the requested currencies</response>
    /// <response code="400">If no currencies are provided or the request is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet(ApiEndpoints.ExchangeRates.GetAllByQueryParams)]
    [SwaggerOperation(
        Summary = "Get exchange rates using GET request",
        Description = "Retrieves exchange rates for specified currencies using query parameters. Convenient for simple requests.",
        OperationId = "GetExchangeRatesGet")]
    [SwaggerResponse(200, "Exchange rates retrieved successfully", typeof(ExchangeRateResponse))
    ]
    [SwaggerResponse(400, "Invalid request - missing currency codes or format violation")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ExchangeRateResponse>> GetExchangeRatesQuery(
        [FromQuery, SwaggerParameter("Comma-separated currency codes (e.g., 'USD,EUR,JPY')", Required = true)] string currencies,
        [FromQuery, SwaggerParameter("Target currency code (defaults to 'CZK')")] string? targetCurrency = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(currencies))
        {
            return BadRequest("Currency codes parameter is required");
        }

        if (!QueryCodesRegex.IsMatch(currencies))
        {
            return BadRequest("Currency codes must be in XXX,YYY,ZZZ format with 3-letter codes");
        }

        var split = currencies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (split.Length > 10)
        {
            return BadRequest("A maximum of 10 currency codes is allowed for GET requests");
        }

        var currencyCodes = split.Select(c => c.Trim().ToUpperInvariant()).ToList();

        var request = new ExchangeRateRequest
        {
            CurrencyCodes = currencyCodes,
            TargetCurrency = targetCurrency?.ToUpperInvariant()
        };

        return await GetExchangeRates(request, cancellationToken);
    }

    /// <summary>
    /// Get information about available exchange rate providers
    /// </summary>
    /// <returns>List of available currency providers</returns>
    /// <response code="200">Returns the list of available providers</response>
    [HttpGet(ApiEndpoints.Providers.GetAll)]
    [SwaggerOperation(
        Summary = "Get available exchange rate providers",
        Description = "Returns information about all available exchange rate providers and their supported currencies.",
        OperationId = "GetAvailableProviders")]
    [SwaggerResponse(200, "Available providers retrieved successfully")]
    public ActionResult<object> GetAvailableProviders()
    {
        var providers = new[]
        {
            new { 
                CurrencyCode = "CZK", 
                Name = "Czech National Bank", 
                Description = "Provides exchange rates with CZK as target currency",
                Endpoint = "https://api.cnb.cz/cnbapi/exrates/daily"
            }
        };

        return Ok(new { Providers = providers });
    }
}