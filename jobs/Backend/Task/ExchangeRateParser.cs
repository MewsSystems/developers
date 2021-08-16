using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateParser
    {
        readonly ExchangeRateParserOptions options;

        public ExchangeRateParser(IOptions<ExchangeRateParserOptions> options)
        {
            this.options = options.Value;
        }

        public IEnumerable<ParsedRate> ParseRates(string exchangeRatesData)
        {
            try
            {
                using var reader = new StringReader(exchangeRatesData);
                // Throw away the first response line with date and ID.
                reader.ReadLine();
                // the rest of the file is a CSV with | as a separator and Czech culture for number styles.
                using var csv = new CsvReader(reader, new CsvConfiguration(options.ParsingCulture) { Delimiter = "|" });
                return csv.GetRecords<ParsedRate>().ToList();
            }
            catch (Exception ex)
            {
                throw new ExchangeRateParsingException("Error parsing exchange rate data.", ex);
            }
        }
        
        public class ParsedRate
        {
            [Index(2)] public int Amount { get; set; }
            [Index(3)] public string Code { get; set; } = "";
            [Index(4)] public decimal Rate { get; set; }
        };
    }

    public sealed record ExchangeRateParserOptions(CultureInfo? parsingCulture = default)
    {
        public CultureInfo ParsingCulture { get; init; } = parsingCulture ?? CultureInfo.GetCultureInfo("cs-CZ");

    }

    public class ExchangeRateParsingException : Exception
    {
        public ExchangeRateParsingException(string message, Exception inner) : base(message, inner)
        {           
        }
    }
}