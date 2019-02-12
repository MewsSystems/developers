using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Loads exchange rates from external source in form of string feed.
    /// </summary>
    public interface IRateFeedSource
    {
        /// <summary>
        /// Asynchronously loads exchange rates from external source in form of string feed.
        /// </summary>
        /// <returns></returns>
        Task<string> LoadRatesFeedAsync();
    }
}
