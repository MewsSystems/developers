using ExchangeRateUpdater.Models.Types;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Persistence;

/// <summary>
/// Represents a repository for retrieving currencies.
/// </summary>
internal interface IExchangeRateRepository
{
    /// <summary>
    /// Retrieves the source currencies from the configuration.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="Currency"/> objects representing the available source currencies.</returns>
    IEnumerable<Currency> GetSourceCurrencies();
}
