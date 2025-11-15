namespace ExchangeRateUpdater.Domain
{
    /// <summary>
    /// Defines a contract for exchange rate providers.
    /// Returns only explicitly defined rates from the source.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Returns exchange rates for the requested currencies.
        /// Only returns rates explicitly defined by the source, 
        /// does not calculate inverted rates.
        /// </summary>
        /// <param name="requestedCurrencies">Currencies to retrieve rates for.</param>
        /// <returns>Collection of exchange rates.</returns>
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> requestedCurrencies);
    }
}
