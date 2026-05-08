namespace ExchangeRateUpdater.Core.Interfaces
{
    /// <summary>
    /// Interface for a client that interacts with the Czech National Bank API to fetch exchange rates.
    /// </summary>
    public interface ICnbApiClient
    {
        /// <summary>
        /// Asynchronously retrieves the latest exchange rates from the Czech National Bank API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing the latest exchange rates.</returns>
        Task<string> GetLatestExchangeRatesAsync();
    }
}