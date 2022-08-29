using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ExchangeRateUpdater.Parsers
{
    public interface ICNBExchangeRateParser
    {
        IEnumerable<CNBExchangeRateItem> ParseCNBResponse(Stream data);
    }

    public class CNBExchangeRateParser : ICNBExchangeRateParser
    {
        private readonly CsvConfiguration csvConfiguration;
        private readonly ICzechNationalBankConfiguration _cnbConfig;

        public CNBExchangeRateParser(ICzechNationalBankConfiguration cnbConfig)
        {
            _cnbConfig = cnbConfig;

            csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = _cnbConfig.CSVResponseDelimiter(),
                HasHeaderRecord = _cnbConfig.CSVExpectingHeaders()
            };
        }

        public IEnumerable<CNBExchangeRateItem> ParseCNBResponse(Stream data)
        {
            using var streamReader = new StreamReader(data);
            using var csv = new CsvReader(streamReader, csvConfiguration);
            csv.Read();//skip first line because it isn't used
            return csv.GetRecords<CNBExchangeRateItem>().ToList();
        }
    }
}
