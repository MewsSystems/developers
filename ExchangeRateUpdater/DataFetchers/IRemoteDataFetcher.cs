namespace ExchangeRateUpdater.DataFetchers
{
    /// <summary>
    /// Defines a contract for fetching raw data from a remote source.
    /// </summary>
    public interface IRemoteDataFetcher
    {
        /// <summary>
        /// Fetches raw data as a string from a remote source.
        /// </summary>
        /// <returns>A string containing raw data ready for parsing.</returns>
        public string FetchData();
    }
}
