namespace ExchangeRateUpdater
{
    /// <summary>
    /// Describes exchange rate data source supporting just get operations.
    /// </summary>
    public interface IExchangeRateDataSource
    {
        /// <summary>
        /// Tries to retrieve exchange rate and amount of units between Czech Crown and other currency identified by <paramref name="currencyCode"/>.
        /// </summary>
        /// <param name="currencyCode">Three-letter ISO 4217 code of the currency.</param>
        /// <param name="amount">Money amount belonging to exchange rate.</param>
        /// <param name="exchangeRate">Exchange rate value belonging to money amount.</param>
        /// <returns><c>true</c> when data source contains exchange rate value and amount, <c>false</c> otherwise.</returns>
        bool TryGet(string currencyCode, out int amount, out decimal exchangeRate);
    }
}
