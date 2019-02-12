using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Loads exchange rate feed from ČNB web.
    /// </summary>
    public class CnbRateFeedSource : IRateFeedSource
    {
        /// <summary>
        /// Asynchronously Loads exchange rate feed from ČNB web.
        /// </summary>
        public async Task<string> LoadRatesFeedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
