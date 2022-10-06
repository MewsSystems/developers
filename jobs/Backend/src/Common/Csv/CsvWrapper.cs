using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Common.Csv
{
    public class CsvWrapper : ICsvWrapper
    {
        public IEnumerable<T> ParseCsv<T>(string csvData, string delimitter, bool hasHeaderRecord, bool skipFirstRow)
        {
            var _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimitter,
                HasHeaderRecord = hasHeaderRecord
            };

            var textReader = new StringReader(csvData);
            using var csv = new CsvReader(textReader, _csvConfiguration);
            // skip the first line because it isn't used
            csv.Read();
            return csv.GetRecords<T>().ToList();
        }
    }
}
