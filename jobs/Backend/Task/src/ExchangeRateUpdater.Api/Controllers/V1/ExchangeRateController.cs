using Asp.Versioning;
using ExchangeRateUpdater.Core.Common.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers.V1;

/// <summary>
/// API controller for retrieving exchange rates from CNB (Czech National Bank)
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ExchangeRateController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExchangeRateController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling CQRS requests</param>
    /// <param name="logger">The logger instance for logging controller operations</param>
    public ExchangeRateController(IMediator mediator,
        ILogger<ExchangeRateController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets the exchange rate for a specific currency pair.
    /// </summary>
    /// <param name="sourceCurrency">Source currency code in ISO-4217 format (e.g., USD)</param>
    /// <param name="targetCurrency">Target currency code in ISO-4217 format (e.g., CZK)</param>
    /// <param name="date">Optional date in ISO-8601 format (yyyy-MM-dd). If not provided, today's rate will be used.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exchange rate details for the specified currency pair</returns>
    /// <response code="200">Returns the exchange rate information</response>
    /// <response code="400">If the request parameters are invalid</response>
    /// <response code="404">If the exchange rate was not found for the given currency pair/date</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(ExchangeRateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ExchangeRateResponse>> GetExchangeRate(
        [FromQuery] string sourceCurrency,
        [FromQuery] string targetCurrency,
        [FromQuery] string? date = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting exchange rate for pair {SourceCurrency}/{TargetCurrency}, date {Date}",
            sourceCurrency,
            targetCurrency,
            date);

        var query = new GetExchangeRateQuery(sourceCurrency, targetCurrency, date);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets exchange rates for multiple currency pairs in a single request.
    /// </summary>
    /// <param name="request">Batch request containing currency pairs and optional date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exchange rates for all requested currency pairs</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/ExchangeRate/batch
    ///     {
    ///        "date": "2023-11-15",
    ///        "currencyPairs": ["USD/CZK", "EUR/CZK", "GBP/CZK"]
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns exchange rates for all requested currency pairs</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("Batch")]
    [ProducesResponseType(typeof(BatchExchangeRateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BatchExchangeRateResponse>> GetBatchExchangeRates(
        [FromBody] BatchRateRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing batch request for {Count} currency pairs, date {Date}",
            request.CurrencyPairs.Count(),
            request.Date);

        var query = new GetBatchExchangeRatesQuery(request);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}