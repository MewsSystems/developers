namespace ExchangeRateUpdater
{
    /// <summary>
    /// Abstraction for fetching raw exchange rate data from a source.
    /// </summary>
    public interface IExchangeRatesSource
    {
        /// <summary>
        /// Fetches the latest exchange rates content as a string.
        /// </summary>
        string GetLatestRatesContent();
    }
}


