using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using REST.Response.Models.Common;
using REST.Response.Models.Areas.ExchangeRates;
using REST.Response.Converters;

namespace REST.Areas.ExchangeRates;

/// <summary>
/// API endpoints for exchange rate operations.
/// </summary>
[ApiController]
[Area("ExchangeRates")]
[Route("api/exchange-rates")]
[Authorize(Roles = "Consumer,Admin")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangeRatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current exchange rates across all providers and currency pairs.
    /// Returns a flat list structure.
    /// </summary>
    [HttpGet("current")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CurrentExchangeRateResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetCurrentRates()
    {
        var query = new GetCurrentExchangeRatesQuery();
        var rates = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<CurrentExchangeRateResponse>>.Ok(
            rates.Select(r => r.ToResponse()),
            "Current exchange rates retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get current exchange rates grouped by provider and base currency.
    /// Returns a nested structure: Provider → Base Currencies → Target Currencies.
    /// </summary>
    [HttpGet("current/grouped")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CurrentExchangeRatesGroupedResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetCurrentRatesGrouped()
    {
        var query = new GetCurrentExchangeRatesQuery();
        var rates = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<CurrentExchangeRatesGroupedResponse>>.Ok(
            rates.ToNestedGroupedResponse(),
            "Current exchange rates (grouped) retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get the latest exchange rate for a currency pair from a specific provider.
    /// </summary>
    /// <param name="sourceCurrency">Source currency code</param>
    /// <param name="targetCurrency">Target currency code</param>
    /// <param name="providerId">Optional provider ID filter</param>
    [HttpGet("latest")]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetLatestRate(
        [FromQuery] string sourceCurrency,
        [FromQuery] string targetCurrency,
        [FromQuery] int? providerId = null)
    {
        var query = new GetLatestExchangeRateQuery(sourceCurrency, targetCurrency, providerId);
        var rateDto = await _mediator.Send(query);

        if (rateDto != null)
        {
            var response = ApiResponse<ExchangeRateResponse>.Ok(
                rateDto.ToResponse(),
                "Latest exchange rate retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ExchangeRateResponse>.NotFound(
            $"No exchange rate found for {sourceCurrency} to {targetCurrency}"
        ));
    }

    /// <summary>
    /// Get all latest exchange rates across all currency pairs.
    /// Returns a flat list structure.
    /// </summary>
    [HttpGet("latest/all")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LatestExchangeRateResponse>>), 200)]
    public async Task<IActionResult> GetAllLatestRates()
    {
        var query = new GetAllLatestExchangeRatesQuery();
        var rates = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<LatestExchangeRateResponse>>.Ok(
            rates.Select(r => r.ToResponse()),
            "All latest exchange rates retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get all latest exchange rates grouped by provider and base currency.
    /// Returns a nested structure: Provider → Base Currencies → Target Currencies.
    /// </summary>
    [HttpGet("latest/all/grouped")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LatestExchangeRatesGroupedResponse>>), 200)]
    public async Task<IActionResult> GetAllLatestRatesGrouped()
    {
        var query = new GetAllLatestExchangeRatesQuery();
        var rates = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<LatestExchangeRatesGroupedResponse>>.Ok(
            rates.ToNestedGroupedResponse(),
            "All latest exchange rates (grouped) retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get historical exchange rates for a currency pair.
    /// Returns a flat list structure.
    /// </summary>
    /// <param name="sourceCurrency">Source currency code</param>
    /// <param name="targetCurrency">Target currency code</param>
    /// <param name="startDate">Start date (YYYY-MM-DD)</param>
    /// <param name="endDate">End date (YYYY-MM-DD)</param>
    /// <param name="providerId">Optional provider ID filter</param>
    [HttpGet("history")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExchangeRateHistoryResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetHistory(
        [FromQuery] string sourceCurrency,
        [FromQuery] string targetCurrency,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] int? providerId = null)
    {
        var query = new GetExchangeRateHistoryQuery(
            sourceCurrency,
            targetCurrency,
            startDate,
            endDate,
            providerId
        );

        var history = await _mediator.Send(query);
        var response = ApiResponse<IEnumerable<ExchangeRateHistoryResponse>>.Ok(
            history.Select(r => r.ToResponse()),
            "Exchange rate history retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get historical exchange rates for a currency pair grouped by provider and base currency.
    /// Returns a nested structure: Provider → Base Currencies → Target Currencies with historical data.
    /// </summary>
    /// <param name="sourceCurrency">Source currency code</param>
    /// <param name="targetCurrency">Target currency code</param>
    /// <param name="startDate">Start date (YYYY-MM-DD)</param>
    /// <param name="endDate">End date (YYYY-MM-DD)</param>
    /// <param name="providerId">Optional provider ID filter</param>
    [HttpGet("history/grouped")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExchangeRateHistoryGroupedResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetHistoryGrouped(
        [FromQuery] string sourceCurrency,
        [FromQuery] string targetCurrency,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] int? providerId = null)
    {
        var query = new GetExchangeRateHistoryQuery(
            sourceCurrency,
            targetCurrency,
            startDate,
            endDate,
            providerId
        );

        var history = await _mediator.Send(query);
        var response = ApiResponse<IEnumerable<ExchangeRateHistoryGroupedResponse>>.Ok(
            history.ToNestedGroupedResponse(),
            "Exchange rate history (grouped) retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Convert an amount from one currency to another.
    /// </summary>
    /// <param name="fromCurrency">Source currency code</param>
    /// <param name="toCurrency">Target currency code</param>
    /// <param name="amount">Amount to convert</param>
    /// <param name="providerCode">Optional provider code (uses latest available if not specified)</param>
    [HttpGet("convert")]
    [ProducesResponseType(typeof(ApiResponse<ConvertCurrencyResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> ConvertCurrency(
        [FromQuery] string fromCurrency,
        [FromQuery] string toCurrency,
        [FromQuery] decimal amount,
        [FromQuery] string? providerCode = null)
    {
        var query = new ConvertCurrencyQuery(
            fromCurrency,
            toCurrency,
            amount,
            null, // ProviderId - will use code instead
            null  // Date
        );

        var conversionResult = await _mediator.Send(query);

        if (conversionResult != null)
        {
            var response = ApiResponse<ConvertCurrencyResponse>.Ok(
                ExchangeRateConverters.ToConversionResponse(
                    conversionResult.SourceCurrencyCode,
                    conversionResult.TargetCurrencyCode,
                    conversionResult.SourceAmount,
                    conversionResult.TargetAmount,
                    conversionResult.EffectiveRate,
                    conversionResult.ValidDate.ToString("yyyy-MM-dd")
                ),
                "Currency conversion completed successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ConvertCurrencyResponse>.NotFound(
            "Exchange rate not found for the specified currency pair"
        ));
    }
}
