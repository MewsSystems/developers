using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb
{
    public sealed class ExchangeRateParser
    {
        public ExchangeRate[] Parse(string data, Currency targetCurrency)
        {
            using (var reader = new StringReader(data))
            {
                var rates = new List<ExchangeRate>();

                string line;
                _ = reader.ReadLine();

                var header = reader.ReadLine();
                ValidateHeader(data, header);

                int currentLine = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    var rate = ParseRow(data, line, targetCurrency, currentLine);
                    rates.Add(rate);

                    currentLine++;
                }

                return rates.ToArray();
            }
        }

        private ExchangeRate ParseRow(string data, string line, Currency targetCurrency, int currentLine)
        {
            var segments = line.Split('|');

            if (segments.Length != 5)
                throw new ExchangeRateProviderException($"Invalid number of fields ({segments.Length}) at line {currentLine}\nData:\n{data}");

            if (!decimal.TryParse(segments[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var amount))
                throw new ExchangeRateProviderException($"Cannot convert {segments[2]} to decimal at line {currentLine}\nData:\n{data}");

            if (!decimal.TryParse(segments[4], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var rate))
                throw new ExchangeRateProviderException($"Cannot convert {segments[4]} to decimal at line {currentLine}\nData:\n{data}");

            var code = segments[3];
            if (string.IsNullOrEmpty(code) || code.Length != 3)
                throw new ExchangeRateProviderException($"Invalid currency code {code} at line {currentLine}\nData:\n{data}");

            return new ExchangeRate(new Currency(code), targetCurrency, decimal.Divide(rate, amount));
        }

        private static void ValidateHeader(string data, string header)
        {
            if (header == null)
                throw new ExchangeRateProviderException($"Missing header on line 2\nData:\n{data}.");
            if (!header.Equals("Country|Currency|Amount|Code|Rate", StringComparison.OrdinalIgnoreCase))
                throw new ExchangeRateProviderException($"Unexpected header on line 2: {header}\nData:\n{data}.");
        }
    }
}
