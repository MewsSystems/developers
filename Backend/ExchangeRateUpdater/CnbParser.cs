using ExchangeRateUpdater.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExchangeRateUpdater
{
    public class CnbParser : IParser
    {
        const string CultureCode = "cs-CZ";

        public Dictionary<string, decimal> Parse(string txtContent)
        {
            string[] lines = txtContent.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries
            );
            Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
            for (int i = 2; i < lines.Length; i++)
            {
                Tuple<string, decimal> tuple = ParseLine(lines[i]);
                string code = tuple.Item1;
                decimal rate = tuple.Item2;
                if (rates.ContainsKey(code))
                {
                    FormatException ex = new FormatException("Duplicate entry for currency code in data source");
                    ex.Data.Add("CurrencyCode", code);
                }
                rates.Add(code, rate);
            }
            return rates;
        }

        private static Tuple<string, decimal> ParseLine(string line)
        {
            string[] items = line.Split(new[] { '|' }, StringSplitOptions.None);
            if (items.Length < 5)
            {
                throw new FormatException("Parsing line failed, expected 5 values");
            }
            string currencyCode = items[3]?.Trim()?.ToUpperInvariant();
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new FormatException("Parsing currency code failed, data source not containig value");
            }

            string amountStr = items[2];
            if (string.IsNullOrEmpty(amountStr) || !int.TryParse(amountStr, out int amount))
            {
                throw new FormatException("Parsing currency amount failed, expecting number");
            }
            string rateStr = items[4]?.Trim();
            if (string.IsNullOrEmpty(rateStr) || !decimal.TryParse(rateStr, NumberStyles.Currency, CultureInfo.GetCultureInfo(CultureCode), out decimal rate))
            {
                throw new FormatException("Parsing currency rate failed, expecting number");
            }

            rate /= amount;
            return new Tuple<string, decimal>(currencyCode, rate);
        }
    }
}
