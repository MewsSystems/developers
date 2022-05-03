namespace ExchangeRateUpdater.Domain
{
    /// <summary>
    /// Repository for exchange rates.
    /// </summary>
    public interface IExchangeRateRepository
    {
        /// <summary>
        /// Initializes the underlying datasource.
        /// </summary>
        /// <returns></returns>
        Task Initialize();

        /// <summary>
        /// Method which resolves whether the repository contains any rates in it.
        /// </summary>
        /// <returns>True if rates are present in the repository.</returns>
        bool Any();

        /// <summary>
        /// Tries to obtain the exchange rate for given <paramref name="currencyCode"/>.
        /// </summary>
        /// <param name="currencyCode">USD, CZK, ....</param>
        /// <returns>Exchange rate or null if not found.</returns>
        ExchangeRate? TryGet(string currencyCode);
    }
}
