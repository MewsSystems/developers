using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Parses exchange rates out of the feed data.
    /// </summary>
    public interface IRateFeedParser
    {
        /// <summary>
        /// Parses exchange rates out of the feed data.
        /// </summary>
        /// <param name="feed">Feed data in string form.</param>
        /// <returns>Collection of <see cref="ExchangeRate"/> parsed from the feed.</returns>
        IEnumerable<ExchangeRate> Parse(string feed);
    }
}
