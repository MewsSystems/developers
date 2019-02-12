using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Parses exchange rate feed from ČNB.
    /// </summary>
    public class CnbRateFeedParser : IRateFeedParser
    {
        public IEnumerable<ExchangeRate> Parse(string feed)
        {
            var lines = feed?.Split('\n') ?? throw new ArgumentNullException(nameof(feed));
            var sourceCurrency = new Currency("CZK");
            for (var i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                if(string.IsNullOrWhiteSpace(line))
                    continue;
                yield return ParseLine(lines[i], sourceCurrency);
            }
        }

        private static ExchangeRate ParseLine(string line, Currency sourceCurrency)
        {
            // země|měna|množství|kód|kurz
            var splitted = line.Split('|');

            return new ExchangeRate(sourceCurrency, new Currency(splitted[3]), decimal.Parse(splitted[4])/decimal.Parse(splitted[2]));
        }
    }
}
