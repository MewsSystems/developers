using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.ExchangeRateParser;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ExchangeRateUpdater.ExchangeRateParsers
{
    public class CnbExchangeRateParser : IExchangeRateParser
    {
        private readonly CsvConfiguration csvConfiguration;

        public CnbExchangeRateParser()
        {
            csvConfiguration = new CsvConfiguration(new CultureInfo("cz-CZ")) { Delimiter = "|" };
        }

        public IEnumerable<ExchangeRateParceItem> Parse(Stream data)
        {
            using (var streamReader = new StreamReader(data))
            using (var csv = new CsvReader(streamReader, csvConfiguration))
            {
                csv.Read();
                return csv.GetRecords<ExchangeRateParceItem>().ToList();
            }                
        }
    }
}
