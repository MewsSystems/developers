using System.Net.Mime;
using Asp.Versioning;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Application.Interfaces;
using ExchangeRateUpdater.Api.Presentation.Contracts.Requests;
using ExchangeRateUpdater.Api.Presentation.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Presentation.Controllers;

/// <summary>
/// API controller for retrieving currency exchange rates from external data providers.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/exchange-rates")]
[Produces(MediaTypeNames.Application.Json)]
public class ExchangeRatesController(IExchangeRateProvider provider) : ApiControllerBase
{
    /// <summary>
    /// Get exchange rates
    /// </summary>
    /// <remarks>
    /// Returns daily exchange rates from the configured data source.
    /// Optionally filter by comma-separated currency codes and/or a specific date.
    /// </remarks>
    [HttpGet(Name = "GetExchangeRates")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExchangeRateResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExchangeRateResponse>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExchangeRateResponse>>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExchangeRateResponse>>), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetExchangeRates(
        [FromQuery] GetExchangeRatesRequest request,
        CancellationToken cancellationToken)
    {
        var dateResult = ParseDate(request.Date);
        var currencyCodes = ParseCurrencyCodes(request.Currencies);

        var result = await provider.GetDailyRatesAsync(dateResult, cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse<IEnumerable<ExchangeRateResponse>>(result.Error);

        var rates = result.Value;
        var filtered = currencyCodes.Length > 0
            ? rates.Where(r => currencyCodes.Contains(r.SourceCurrency.Code, StringComparer.OrdinalIgnoreCase))
            : rates;

        var response = filtered.Select(r => new ExchangeRateResponse(
            r.SourceCurrency.Code,
            r.TargetCurrency.Code,
            r.Rate,
            r.Amount,
            r.ValidFor.ToString("yyyy-MM-dd"))).ToArray();

        if (currencyCodes.Length > 0 && response.Length == 0)
        {
            var requestedCurrencies = string.Join(", ", currencyCodes);

            return ToErrorResponse<IEnumerable<ExchangeRateResponse>>(
                ExchangeRateErrors.CurrencyNotFound(requestedCurrencies));
        }

        return Ok(ApiResponse<IEnumerable<ExchangeRateResponse>>.Success(response));
    }

    /// <summary>
    /// Get exchange rate by currency code
    /// </summary>
    /// <remarks>
    /// Returns the daily exchange rate for a specific ISO 4217 currency codefrom the configured data source.
    /// </remarks>
    [HttpGet("{currencyCode}", Name = "GetExchangeRateByCode")]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<ExchangeRateResponse>), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetExchangeRate(
        GetExchangeRateByCodeRequest request,
        CancellationToken cancellationToken)
    {
        var dateResult = ParseDate(request.Date);

        var result = await provider.GetDailyRatesAsync(dateResult, cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse<ExchangeRateResponse>(result.Error);

        var rate = result.Value.FirstOrDefault(r =>
            r.SourceCurrency.Code.Equals(request.CurrencyCode, StringComparison.OrdinalIgnoreCase));

        if (rate is null)
            return ToErrorResponse<ExchangeRateResponse>(ExchangeRateErrors.CurrencyNotFound(request.CurrencyCode));

        var response = new ExchangeRateResponse(
            rate.SourceCurrency.Code,
            rate.TargetCurrency.Code,
            rate.Rate,
            rate.Amount,
            rate.ValidFor.ToString("yyyy-MM-dd"));

        return Ok(ApiResponse<ExchangeRateResponse>.Success(response));
    }

    private static string[] ParseCurrencyCodes(string? currencies) =>
        string.IsNullOrWhiteSpace(currencies)
            ? []
            : currencies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static DateOnly? ParseDate(string? date) =>
        string.IsNullOrWhiteSpace(date)
            ? null
            : DateOnly.ParseExact(date, "yyyy-MM-dd");
}
