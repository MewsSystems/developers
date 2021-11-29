using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace ExchangeRateUpdater.CsvParser;

public interface ICsvFactory
{
    /// <summary>
    /// Create CsvReader
    /// </summary>
    IReader CreateReader(TextReader textReader, CsvConfiguration configuration);
}