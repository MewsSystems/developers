using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class TextCnbParser : ITextParser<ExchangeRate>
    {
        private const char NewLineCharacter = '\n';
        private const char ColumnDelimiter = '|';
        private const int CurrencyCodeColumnIndex = 3;
        private const int ExchangeRateColumnIndex = 4;
        private const int ExpectedColsCount = 5;
        private const int SourceCurrencyAmountColumnIndex = 2;
        private const string DefaultTargetCurrencyCode = "CZK";
        private const string HeaderRegex = @"^\d{2}.\d{2}.\d{4} #\d*$";
        private const string Header = "země|měna|množství|kód|kurz";

        public bool TryParse(string source, out IEnumerable<ExchangeRate> result)
        {
            if (string.IsNullOrEmpty(source)) throw new ArgumentException($"{nameof(source)} cannot be empty");
            var res = new List<ExchangeRate>();
            result = res;
            var lines = source.Split(new char[] { NewLineCharacter }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) return false;

            foreach (var line in lines)
            {
                if (IsHeaderPart(line)) continue;
                var cols = line.Split(new char[] { ColumnDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length != ExpectedColsCount) return false;

                var currencyCode = cols[CurrencyCodeColumnIndex];
                if (!decimal.TryParse(cols[ExchangeRateColumnIndex], out var exchangeRate)) {
                    return false;
                };
                if (!int.TryParse(cols[SourceCurrencyAmountColumnIndex], out var sourceCurrencyAmount))
                {
                    return false;
                };
                if (sourceCurrencyAmount > 1)
                {
                    exchangeRate /= sourceCurrencyAmount;
                }

                res.Add(new ExchangeRate
                (
                    new Currency(currencyCode),
                    new Currency(DefaultTargetCurrencyCode),
                    exchangeRate
                ));
            }
            return true;
        }

        private bool IsHeaderPart(string line)
        {
            return line == Header || Regex.IsMatch(line, HeaderRegex);
        }
    }
}
