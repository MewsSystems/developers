using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Loads exchange rate feed from ČNB web.
    /// </summary>
    public class CnbRateFeedSource : IRateFeedSource
    {
        private readonly string url;

        /// <summary>
        /// Creates new instance of <see cref="CnbRateFeedSource"/>.
        /// </summary>
        /// <param name="url">Actual url for getting the feed.</param>
        public CnbRateFeedSource(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Loads rate feed from ČNB using url provided to the constructor.
        /// </summary>
        public async Task<string> LoadRatesFeedAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Getting rates failed: {ex.Message}");
                return null;
            }
        }
    }
}
