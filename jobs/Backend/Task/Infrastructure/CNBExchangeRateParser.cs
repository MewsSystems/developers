using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace ExchangeRateUpdater.Infrastructure
{
    internal sealed class CNBExchangeRateParser : IExchangeRateParser
    {
        private readonly CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.Trim().ToLower(),
            Delimiter = "|",            
        };

        public IEnumerable<ExchangeRate> Parse(string exchangeRate)
        {
            using(var reader = new StringReader(exchangeRate))
            using (var csvReader = new CsvReader(reader, config))
            {                
                var dateInfo = reader.ReadLine();
                var metadata = ParseMetadata(dateInfo);

                var records = csvReader.GetRecords<ExchangeRateRecord>();
                return records.Select(records => new ExchangeRate(
                    new Currency("CZK"),
                    new Currency(records.Code),
                    records.Rate / records.Amount))
                    .ToArray();
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

        private record ExchangeRateMetadata(DateTime DateTime, int Identifier);
    }
}
