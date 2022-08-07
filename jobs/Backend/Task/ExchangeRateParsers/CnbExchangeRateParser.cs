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

        public IEnumerable<ExchangeRateParceItem> Parce(string data)
        {
            var byteArray = Encoding.UTF8.GetBytes(data);
            using (var memoryStream = new MemoryStream(byteArray))
            using (var streamReader = new StreamReader(memoryStream))
            using (var csv = new CsvReader(streamReader, csvConfiguration))
            {
                csv.Read();
                return csv.GetRecords<ExchangeRateParceItem>().ToList();
            }                
        }
    }
}
