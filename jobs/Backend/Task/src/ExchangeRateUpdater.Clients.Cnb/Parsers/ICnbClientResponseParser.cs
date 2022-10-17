using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Clients.Cnb.Parsers;

/// <summary>
/// Cnb client response parser.
/// </summary>
public interface ICnbClientResponseParser
{
    /// <summary>
    /// Extracts each line by defined specs.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Returns an exchange rate.</returns>
    ExchangeRateDto? ExtractExchangeRate(string line);
    
    /// <summary>
    /// Extracts date by defined specs.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Returns a date.</returns>
    DateTime? ExtractDate(string line);
}