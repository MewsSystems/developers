using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services.RateExporters;

/// <summary>
///     Defines a contract for exporting exchange rate data to different output targets.
///     Implementations can export to console, database, files, or other destinations.
/// </summary>
public interface IExchangeRateExporter
{
    Task ExportExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates);
}