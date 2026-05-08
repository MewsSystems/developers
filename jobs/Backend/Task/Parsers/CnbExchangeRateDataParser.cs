namespace ExchangeRateUpdater.Parsers
{
    using ExchangeRateUpdater.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public sealed class CnbExchangeRateDataParser : IExchangeRateDataParser
    {
        private const int LinesToSkip = 2;
        private const int ExpectedColumnCount = 5;
        private const char ColumnDelimiter = '|';
        private static readonly Currency CzkCurrency = new("CZK");


        public IEnumerable<ExchangeRate> Parse(string data, IEnumerable<Currency> currencies)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (currencies == null) throw new ArgumentNullException(nameof(currencies));

            var currencyCodesSet = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            var lines = data
                .Split(["\r\n", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries);

            var results = lines
                .Skip(LinesToSkip)
                .Select(ParseLine)
                .Where(rate => rate != null && currencyCodesSet.Contains(rate.SourceCurrency.Code))
                .Select(r => r!)
                .ToList();

            return results;
        }

        private static ExchangeRate? ParseLine(string line)
        {
            var columns = line.Split(ColumnDelimiter);

            if (columns.Length != ExpectedColumnCount)
            {
                return null;
            }

            var currencyCode = columns[3].Trim();
            var amountText = columns[2].Trim();
            var rateText = columns[4].Trim();

            if (!decimal.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount) || amount <= 0)
            {
                return null;
            }

            if (!decimal.TryParse(rateText, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate) || rate <= 0)
            {
                return null;
            }

            var normalizedRate = rate / amount;

            return new ExchangeRate(sourceCurrency: new Currency(currencyCode), targetCurrency: CzkCurrency, value: normalizedRate);
        }
    }
}
