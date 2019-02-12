using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Parses exchange rate feed from ČNB.
    /// </summary>
    public class CnbRateFeedParser : IRateFeedParser
    {
        /// <summary>
        /// Parses exchange rate feed from ČNB.
        /// </summary>
        public IEnumerable<ExchangeRate> Parse(string feed)
        {
            throw new NotImplementedException();
        }
    }
}
