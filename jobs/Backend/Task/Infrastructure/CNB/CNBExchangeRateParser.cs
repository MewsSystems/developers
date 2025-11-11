using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.CNB
{
    internal interface IExchangeRateParser
    {
        (ExchangeRateMetadata Metadata, IEnumerable<ExchangeRate> Records) Parse(string exchangeRate);
    }

    internal record ExchangeRateMetadata(DateTime DateTime, int Identifier);

    internal sealed class CNBExchangeRateParser(ILogger<CNBExchangeRateParser> logger) : IExchangeRateParser
    {
        private const string CheckCurrencyCode = "CZK";
        private readonly CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.Trim().ToLower(),
            Delimiter = "|",            
        };
        private readonly ILogger<CNBExchangeRateParser> logger = logger;

        public (ExchangeRateMetadata Metadata, IEnumerable<ExchangeRate> Records) Parse(string exchangeRate)
        {
            logger.LogTrace("Parsing exchange rate data");

            using (var reader = new StringReader(exchangeRate))
            using (var csvReader = new CsvReader(reader, config))
            {                
                var dateInfo = reader.ReadLine();
                var metadata = ParseMetadata(dateInfo);

                logger.LogTrace("Parsed metadata: {Date} {Identifier}", metadata.DateTime, metadata.Identifier);

                var records = csvReader.GetRecords<ExchangeRateRecord>();
                return (metadata, records.Select(records => new ExchangeRate(
                    new Currency(CheckCurrencyCode),
                    new Currency(records.Code),
                    records.Rate / records.Amount))
                    .ToArray());
            }
        }

        private static ExchangeRateMetadata ParseMetadata(string dateInfo)
        {
            // Example: dateInfo = "16 May 2025 #93"
            DateTime? parsedDate = null;
            int? identifier = null;

            if (!string.IsNullOrWhiteSpace(dateInfo))
            {
                var parts = dateInfo.Split('#');
                var datePart = parts[0].Trim();

                if (DateTime.TryParseExact(datePart, "d MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    parsedDate = dt;

                if (parts.Length > 1 && int.TryParse(parts[1].Trim(), out var id))
                    identifier = id;

                return new ExchangeRateMetadata(parsedDate ?? DateTime.MinValue, identifier ?? 0);
            }

            return new ExchangeRateMetadata(DateTime.MinValue, 0);

        }

        private record class ExchangeRateRecord(string Country, string Currency, decimal Amount, string Code, decimal Rate);
    }
}
