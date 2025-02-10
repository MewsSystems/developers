namespace ExchangeRateUpdater.Domain.Models;

/// <summary>
/// Represents exchange rates along with the date and additional response details.
/// </summary>
/// <param name="ExchangeRates">The collection of exchange rates for the specified date.</param>
/// <param name="Date">The date for which the exchange rates are valid in ISO 8601 format (yyyy-MM-dd).</param>
/// <param name="MissingCurrencies">A list of requested currencies that were not found.</param>
/// <param name="Message">An optional message providing additional information about the response.</param>
public sealed record ExchangeRateResponse(
    IEnumerable<ExchangeRate> ExchangeRates,
    string Date,
    IEnumerable<string>? MissingCurrencies = null,
    string? Message = null);
