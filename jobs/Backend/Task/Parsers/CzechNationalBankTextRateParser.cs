using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Parsers
{
    public class CzechNationalBankTextRateParser : IExchangeRateParser
    {
        private readonly int _expectedColumns;
        private readonly ILogger<CzechNationalBankTextRateParser> _logger;

        public CzechNationalBankTextRateParser(int expectedColumns, ILogger<CzechNationalBankTextRateParser> logger)
        {
            _expectedColumns = expectedColumns;
            _logger = logger;
        }

        public IEnumerable<ExchangeRate> Parse(string raw, IEnumerable<Currency> filter, Currency sourceCurrency)
        {
            var parsedRates = new List<ExchangeRate>();
            var filterSet = new HashSet<string>(
                filter.Select(c => c.Code),
                StringComparer.OrdinalIgnoreCase);

            var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            _logger.LogDebug("Parsing CNB text: total lines (including header): {LineCount}", lines.Length);

            for (int i = 2; i < lines.Length; i++) // skip header + date line
            {
                var parts = lines[i].Split('|');

                if (parts.Length < _expectedColumns)
                {
                    _logger.LogWarning("Skipping line {LineIndex}: expected {ExpectedColumns} columns but got {ActualColumns}",
                        i, _expectedColumns, parts.Length);
                    continue;
                }

                string code = parts[3].Trim();
                if (!filterSet.Contains(code))
                {
                    _logger.LogDebug("Skipping currency {CurrencyCode} as it's not in the filter list.", code);
                    continue;
                }

                try
                {
                    int amount = int.Parse(parts[2].Trim());
                    decimal rate = decimal.Parse(
                        parts[4].Trim().Replace(',', '.'),
                        CultureInfo.InvariantCulture);

                    decimal normalized = rate / amount;

                    var exchangeRate = new ExchangeRate(sourceCurrency, new Currency(code), normalized);
                    parsedRates.Add(exchangeRate);

                    _logger.LogDebug("Parsed: {Source}/{Target} = {Value}",
                        sourceCurrency, code, normalized);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to parse line {LineIndex}: {LineContent}", i, lines[i]);
                }
            }

            _logger.LogInformation("Finished parsing. Extracted {ParsedCount} exchange rates.", parsedRates.Count);
            return parsedRates;
        }
    }
}
