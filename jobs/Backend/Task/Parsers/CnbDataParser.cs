using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ExchangeRateUpdater.Parsers
{
    /// <summary>
    /// Parses CNB daily exchange rates text format.
    /// Format: Country|Currency|Amount|Code|Rate
    /// Example: USA|dollar|1|USD|24.123
    /// </summary>
    public class CnbDataParser : IExchangeRateDataParser
    {
        private const int ExpectedColumnCount = 5;
        private const int AmountIndex = 2;
        private const int CodeIndex = 3;
        private const int RateIndex = 4;
        private static readonly Currency CzkCurrency = new("CZK");

        public IEnumerable<ExchangeRate> Parse(string rawData, IEnumerable<Currency> targetCurrencies)
        {
            if (string.IsNullOrWhiteSpace(rawData))
                throw new ExchangeRateException("CNB data is empty.");

            // enhances performance and ensures that the filtering process is accurate and efficient.
            // HashSet for O(1) lookup performance instead of O(n) with List.Contains--> Checking if currency is requested happens for every parsed line
            var currencyCodesSet = new HashSet<string>(
                targetCurrencies.Select(c => c.Code),
                StringComparer.OrdinalIgnoreCase);

            //Parse all lines and then filter
            return ParseLines(rawData)
                .Where(rate => currencyCodesSet.Contains(rate.SourceCurrency.Code))
                .ToList();
        }

        private IEnumerable<ExchangeRate> ParseLines(string rawData)
        {
            var lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // first 2 lines: date and column headers
            return lines.Skip(2)
                .Select(ParseLine)
                .Where(rate => rate != null)
                .Cast<ExchangeRate>();
        }

        private ExchangeRate? ParseLine(string line)
        {
            try
            {
                var columns = line.Split('|');

                if (columns.Length != ExpectedColumnCount)
                    return null;

                var currencyCode = columns[CodeIndex].Trim();
                var amount = ParseDecimal(columns[AmountIndex]);
                var rate = ParseDecimal(columns[RateIndex]);

                // CNB rates are: {amount} {currency} = {rate} CZK
                // We want: 1 {currency} = {rate/amount} CZK. We normalize it to 1 unit.
                var normalizedRate = rate / amount;

                return new ExchangeRate(
                    sourceCurrency: new Currency(currencyCode),
                    targetCurrency: CzkCurrency,
                    value: normalizedRate);
            }
            catch
            {
                // Skip malformed lines
                return null;
            }
        }

        private decimal ParseDecimal(string value)
        {
            // CNB uses comma as decimal separator
            var normalized = value.Trim().Replace(',', '.');
            return decimal.Parse(normalized, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}