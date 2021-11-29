using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace ExchangeRateUpdater.CsvParser;

public class CsvFactory : ICsvFactory
{
    public IReader CreateReader(TextReader textReader, CsvConfiguration configuration)
    {
        return new CsvReader(textReader, configuration);
    }
}