using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Abstract;
using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Model;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Cnb
{
    public class CnbRatesTxtParser : ICnbRatesParser
    {
        private const int AmountIndex = 2;
        private const int CurrencyCodeIndex = 3;
        private const int RateIndex = 4;

        public IEnumerable<CnbRate> Parse(string contents) => contents
            .Split('\n')
            .Skip(2)
            .SelectMany(ParseLine);

        private static IEnumerable<CnbRate> ParseLine(string line)
        {
            var parts = line.Split('|');
            if (parts.Length != 5) yield break;

            int amount;
            if (!int.TryParse(parts[AmountIndex], out amount)) yield break;

            decimal rate;
            if (!decimal.TryParse(parts[RateIndex], out rate)) yield break;

            yield return new CnbRate(
                parts[CurrencyCodeIndex],
                amount,
                rate);
        }
    }
}
