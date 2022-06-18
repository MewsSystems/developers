using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;
using System.Globalization;

namespace ExchangeRateUpdated.Service.Parsers
{
    public class CnbCsvParser : ICnbCsvParser
    {
        private static CsvConfiguration _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "|"
        };

        public Result<IEnumerable<CnbExchangeRateRecord>> TryParseExchangeRates(Stream stream)
        {
            return Result.Try(() => ParseExchangeRates(stream));
        }

        private IEnumerable<CnbExchangeRateRecord> ParseExchangeRates(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, _csvConfiguration);

            csv.Read(); // skip the first line which is info about current DateTime

            var records = csv.GetRecords<CnbExchangeRateRecord>().ToList();

            return records;
        }
    }
}
