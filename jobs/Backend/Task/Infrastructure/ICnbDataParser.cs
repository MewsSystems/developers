using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Interface for parsing CNB exchange rate data.
/// </summary>
public interface ICnbDataParser
{
    /// <summary>
    /// Parses raw CNB data into exchange rate DTOs.
    /// </summary>
    /// <param name="rawData">Raw text data from CNB API.</param>
    /// <returns>Collection of parsed exchange rate data.</returns>
    IEnumerable<CnbExchangeRateDto> Parse(string rawData);
}
