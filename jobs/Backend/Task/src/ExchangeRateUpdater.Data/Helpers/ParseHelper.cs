using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Helpers
{
    /// <summary>
    /// Helper class for parsing exchange rate data.
    /// Marked as internal to discourage direct use; the public API is IExchangeRateProvider.
    /// Tests access it via InternalsVisibleTo attribute.
    /// </summary>
    internal static class ParseHelper
    {
        /// <summary>
        /// Parses exchange rates from the source content.
        /// Internal method - only called from IExchangeRateProvider implementation and tests.
        /// </summary>
        internal static IEnumerable<ExchangeRate> ParseRates(string content, IEnumerable<Currency> currencies)
        {
            var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 3)
                return Enumerable.Empty<ExchangeRate>();

            var codesInInput = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);
            var targetCurrencyCode = "CZK";

            var result = new List<ExchangeRate>();

            try
            {
                // The CNB data format:
                // Line 1: Date (e.g., "14 Nov 2025")
                // Line 2: Reference number (e.g., "#222")
                // Line 3: Header (e.g., "Country|Currency|Amount|Code|Rate")
                // Line 4+: Data rows

                var csvLines = lines.Skip(2).ToList();

                if (csvLines.Count == 0)
                    return Enumerable.Empty<ExchangeRate>();

                var csvContent = string.Join(Environment.NewLine, csvLines);

                using var reader = new StringReader(csvContent);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = "|",
                    HasHeaderRecord = true,
                    HeaderValidated = null,  // Disable header validation
                };
                using var csv = new CsvReader(reader, config);

                // Register the class map for proper field mapping
                csv.Context.RegisterClassMap<ExchangeRateRecordMap>();

                // Read all records - materialize the enumerable to ensure parsing happens
                var records = csv.GetRecords<ExchangeRateRecord>().ToList();

                foreach (var record in records)
                {
                    try
                    {
                        // Skip records with invalid data
                        if (record?.Code == null || string.IsNullOrWhiteSpace(record.Code))
                            continue;

                        // Validate that both source currency and target currency (CZK) are requested
                        if (!codesInInput.Contains(record.Code) || !codesInInput.Contains(targetCurrencyCode))
                            continue;

                        // Calculate unit rate: rate / amount
                        var unitRate = record.Rate / record.Amount;
                        var sourceCurrency = new Currency(record.Code);
                        var targetCurrency = new Currency(targetCurrencyCode);

                        result.Add(new ExchangeRate(sourceCurrency, targetCurrency, unitRate));
                    }
                    catch (FormatException)
                    {
                        // Skip records with format issues (invalid data types)
                        continue;
                    }
                    catch (OverflowException)
                    {
                        // Skip records with numeric overflow
                        continue;
                    }
                }
            }
            catch (ArgumentException)
            {
                // Invalid CSV content structure - return empty results
                return Enumerable.Empty<ExchangeRate>();
            }
            catch (OperationCanceledException)
            {
                // Parsing was cancelled - return empty results
                return Enumerable.Empty<ExchangeRate>();
            }

            return result;
        }

        /// <summary>
        /// Class map for CsvHelper to properly map CSV columns to ExchangeRateRecord properties.
        /// Maps by column index (0-based) instead of by header name for better flexibility.
        /// </summary>
        private sealed class ExchangeRateRecordMap : ClassMap<ExchangeRateRecord>
        {
            public ExchangeRateRecordMap()
            {
                // Map by index instead of header name for more flexibility
                Map(m => m.Country).Index(0);
                Map(m => m.Currency).Index(1);
                Map(m => m.Amount).Index(2);
                Map(m => m.Code).Index(3);
                Map(m => m.Rate).Index(4);
            }
        }
    }
}
