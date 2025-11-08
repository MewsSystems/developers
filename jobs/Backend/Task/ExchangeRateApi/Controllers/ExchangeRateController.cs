using ExchangeRateApi.Models;
using ExchangeRateApi.Models.Validators;
using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeRateApi.Controllers;

[ApiController]
[SwaggerTag("Exchange Rate operations for retrieving currency exchange rates")]
public class ExchangeRateController : ControllerBase
{
	private readonly IExchangeRateService _exchangeRateService;
	private readonly ILogger<ExchangeRateController> _logger;
	private static readonly CurrencyQueryParamValidator _currencyQueryParamValidator = new();
	private static readonly ExchangeRateRequestValidator _exchangeRateRequestValidator = new();
	private const string DefaultTargetCurrency = "CZK";

	public ExchangeRateController(
		IExchangeRateService exchangeRateService,
		ILogger<ExchangeRateController> logger)
	{
		_exchangeRateService = exchangeRateService;
		_logger = logger;
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
	[SwaggerResponse(200, "Exchange rates retrieved successfully", typeof(ExchangeRateResponse))]
	[SwaggerResponse(400, "Invalid request - missing or invalid currency codes", typeof(ErrorResponse))]
	[SwaggerResponse(500, "Internal server error", typeof(ErrorResponse))]
	public async Task<ActionResult<ExchangeRateResponse>> GetExchangeRates(
	[FromBody] ExchangeRateRequest request,
	CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Received request for exchange rates with {Count} currencies", request.CurrencyCodes.Count);

		if (request.CurrencyCodes == null || !request.CurrencyCodes.Any())
		{
			_logger.LogWarning("Exchange rate request received with empty currency codes");
			throw new ArgumentException("At least one currency code must be provided");
		}

		var validationResult = await _exchangeRateRequestValidator.ValidateAsync(request, cancellationToken);
		
		if (ToBadRequestIfInvalid(validationResult) is { } badRequest) return badRequest;

		var requestedCurrencies = GetRequestedCurrencies(request);

		var targetCurrency = request.TargetCurrency?.ToUpperInvariant() ?? DefaultTargetCurrency;

		var currencyRates = await GetExchangeRatesForCurrenciesAsync(targetCurrency, requestedCurrencies, cancellationToken);

		_logger.LogInformation("Successfully retrieved {Count} exchange rates for target currency {TargetCurrency}",
			currencyRates.Count(), targetCurrency);

		var response = new ExchangeRateResponse
		{
			TargetCurrency = targetCurrency,
			Rates = currencyRates.Select(rate => new ExchangeRateDto
			{
				SourceCurrency = rate.SourceCurrency.Code,
				TargetCurrency = rate.TargetCurrency.Code,
				Rate = rate.Value,
				ValidFor = rate.ValidFor
			}).ToList()
		};

		return Ok(response);
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
	[SwaggerResponse(200, "Exchange rates retrieved successfully", typeof(ExchangeRateResponse))]
	[SwaggerResponse(400, "Invalid request - missing currency codes or format violation", typeof(ErrorResponse))]
	[SwaggerResponse(500, "Internal server error", typeof(ErrorResponse))]
	public async Task<ActionResult<ExchangeRateResponse>> GetExchangeRatesQuery(
		[FromQuery, SwaggerParameter("Comma-separated currency codes (e.g., USD,EUR,JPY)", Required = true)] string currencies,
		[FromQuery, SwaggerParameter("Target currency code (defaults to 'CZK')")] string? targetCurrency = null,
		CancellationToken cancellationToken = default)
	{
		var validationResult = _currencyQueryParamValidator.Validate(currencies);

		var validationErrors = HandleValidationResult(validationResult);

		if (ToBadRequestIfInvalid(validationResult) is { } badRequest) return badRequest;

		var request = new ExchangeRateRequest
		{
			CurrencyCodes = GetCurrenctyCodesFromQueryParams(currencies),
			TargetCurrency = targetCurrency?.ToUpperInvariant()
		};

		return await GetExchangeRates(request, cancellationToken);
	}

	/// <summary>
	/// Get information about available exchange rate providers
	/// </summary>
	/// <returns>Just some hardcoded list of available currency providers</returns>
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
			},
			new {
				CurrencyCode = "USD",
				Name = "FED",
				Description = "Provides exchange rates with USD as target currency",
				Endpoint = "Mock data for testing purposes"
			}
		};

		return Ok(new { Providers = providers });
	}

	private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesForCurrenciesAsync(string targetCurrency, IEnumerable<Currency> currencies, CancellationToken cancellationToken)
	{
		if (!currencies.Any())
		{
			_logger.LogWarning("No valid currency codes provided after filtering");
			throw new ArgumentException("No valid currency codes provided");
		}

		var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(targetCurrency, currencies, cancellationToken);

		return exchangeRates;
	}

	private List<string> GetCurrenctyCodesFromQueryParams(string currencies)
	{
		return currencies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Select(c => c.Trim().ToUpperInvariant())
			.ToList();
	}

	private ErrorResponse? HandleValidationResult(FluentValidation.Results.ValidationResult validationResult)
	{
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
			_logger.LogWarning("Validation failed for request: {Errors}", string.Join(", ", errors));
			return new  ErrorResponse
			{
				Error = string.Join("; ", errors)
			};
		}

		return null;
	}

	private List<Currency> GetRequestedCurrencies(ExchangeRateRequest request)
	{
		return request.CurrencyCodes
	   .Where(code => !string.IsNullOrWhiteSpace(code))
	   .Select(code => new Currency(code.ToUpperInvariant()))
	   .ToList();
	}

	private ActionResult? ToBadRequestIfInvalid(FluentValidation.Results.ValidationResult result)
	{
		var error = HandleValidationResult(result);
		return error == null ? null : BadRequest(error);
	}
}