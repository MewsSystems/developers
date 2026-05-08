using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Parsers.Models;

namespace ExchangeRateUpdater.Parsers
{
    // Parser for CNB exchange rate data in pipe-separated format.
    public class CnbDataParser : ICnbDataParser
    {
        private const string Header = "Country|Currency|Amount|Code|Rate";
        private const char Separator = '|';
        private const int ColumnCount = 5;

        private readonly ILogger<CnbDataParser> _logger;

        public CnbDataParser(ILogger<CnbDataParser> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Method's implementation: parses raw CNB exchange rate data and returns a collection of parsed exchange rate records
        public IEnumerable<CnbExchangeRateData> Parse(string rawData)
        {
            if (string.IsNullOrWhiteSpace(rawData))
            {
                _logger.LogError("Cannot parse empty or null data");
                throw new CnbDataParsingException("Cannot parse empty or null data.");
            }

            var lines = rawData
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .ToArray();

            if (lines.Length < 3)
            {
                _logger.LogError("Invalid data format. Expected at least 3 lines, got {LineCount}", lines.Length);
                throw new CnbDataParsingException($"Invalid data format. Expected at least 3 lines (date header, column headers, data), but got {lines.Length}.", rawData);
            }

            // Line 0: Date header (skip)
            // Line 1: Column headers (validate)
            ValidateHeader(lines[1]);

            // Lines 2+: Data rows
            var result = new List<CnbExchangeRateData>();

            // Index i starts from 2 to skip date header and column headers
            for (int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineNumber = i + 1; // line number in the txt file for error reporting

                try
                {
                    var data = ParseDataRow(line);
                    result.Add(data);
                }
                catch (CnbDataParsingException ex) when (ex.LineNumber == null)
                {
                    _logger.LogError(ex, "Failed to parse line {LineNumber}: {ErrorMessage}", lineNumber, ex.Message);
                    // Re-throw with line number context
                    throw new CnbDataParsingException(
                        ex.Message,
                        ex.RawData ?? line,
                        lineNumber,
                        ex.InnerException);
                }
                catch (Exception ex) when (ex is not CnbDataParsingException)
                {
                    _logger.LogError(ex, "Unexpected error parsing line {LineNumber}", lineNumber);
                    throw new CnbDataParsingException(
                        $"Failed to parse line {lineNumber}: {ex.Message}",
                        line,
                        lineNumber,
                        ex);
                }
            }

            return result;
        }

        private void ValidateHeader(string headerLine)
        {
            if (!string.Equals(headerLine, Header, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("Invalid column headers. Expected '{ExpectedHeader}', got '{ActualHeader}'", Header, headerLine);
                throw new CnbDataParsingException($"Invalid column headers. Expected '{Header}', but got '{headerLine}'.");
            }
        }

        private static CnbExchangeRateData ParseDataRow(string line)
        {
            var columns = line.Split(Separator);

            if (columns.Length != ColumnCount)
            {
                throw new CnbDataParsingException($"Invalid number of columns. Expected {ColumnCount}, but got {columns.Length}.", line);
            }

            var country = columns[0].Trim();
            var currencyName = columns[1].Trim();
            var amountStr = columns[2].Trim();
            var code = columns[3].Trim();
            var rateStr = columns[4].Trim();

            // Parse Amount
            if (!int.TryParse(amountStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount))
            {
                throw new CnbDataParsingException($"Failed to parse Amount '{amountStr}' as integer.", line);
            }
            if (amount <= 0)
            {
                throw new CnbDataParsingException($"Amount must be positive, but got {amount}.", line);
            }

            // Parse Rate
            if (!decimal.TryParse(rateStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var rate))
            {
                throw new CnbDataParsingException($"Failed to parse Rate '{rateStr}' as decimal.", line);
            }
            if (rate <= 0)
            {
                throw new CnbDataParsingException($"Rate must be positive, but got {rate}.", line);
            }

            // Validate currency code
            if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            {
                throw new CnbDataParsingException($"Invalid currency code '{code}'. Expected 3-letters code.", line);
            }

            return new CnbExchangeRateData(
                Country: country,
                CurrencyName: currencyName,
                Amount: amount,
                Code: code.ToUpper(),
                Rate: rate
            );
        }
    }
}
