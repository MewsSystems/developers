using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Exceptions;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Parsers;

/// <summary>
/// Parses a raw daily exchange rates response into a structured model.
/// </summary>
public interface IDailyExchangeRatesResponseParser
{
    /// <summary>
    /// Parses the raw response content.
    /// </summary>
    /// <param name="rawData">Raw response body returned by the provider.</param>
    /// <returns>The parsed daily exchange rates.</returns>
    /// <exception cref="CzechNationalBankParsingException">
    /// Thrown when the response format is invalid or unsupported.
    /// </exception>
    DailyExchangeRatesResponse Parse(string rawData);
}