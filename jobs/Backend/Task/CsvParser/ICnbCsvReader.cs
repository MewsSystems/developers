using System.Collections.Generic;
using System.IO;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.CsvParser;

/// <summary>
/// Csv reader for CNB data
/// </summary>
public interface ICnbCsvReader
{
    /// <summary>
    /// Convert csv data stream to CnbExchangeRateModels
    /// </summary>
    IEnumerable<CnbExchangeRateModel> GetRecords(Stream stream);
}