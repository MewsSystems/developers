using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater.Cnb
{
    public class CnbDataParser
    {
        public IEnumerable<ExchangeRate> ParseData(string data)
        {
            return data?.Split('\n')
                .Skip(2)
                .Select(line => line.Trim())
                .Select(line => line.Split('|').Skip(3).Take(2))
                .Where(line => line.Any())
                .Select(line => ParseLine(line))
                .ToArray() ?? new ExchangeRate[0];
        }

        internal static ExchangeRate ParseLine(IEnumerable<string> line)
        {
            return new ExchangeRate(new Currency("CZK"),
                new Currency(line?.FirstOrDefault()?.ToUpper()),
                decimal.Parse(line?.Skip(1).FirstOrDefault() ?? "0", new CultureInfo("cs-CZ")));
        }
    }
}