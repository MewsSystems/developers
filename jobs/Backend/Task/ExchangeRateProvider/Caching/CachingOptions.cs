namespace ExchangeRateUpdater.Caching
{

    public class CachingOptions
    {
        /// <summary>
        /// Defines time, how long are not actual data stored in cache, 
        /// before the client tries to retrieve new data from data source.
        /// </summary>
        public TimeSpan OutdatedDataCacheExpiration { get; set; } = TimeSpan.FromMinutes(5);
    }
}