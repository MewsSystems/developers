namespace ExchangeRateUpdater.ServiceContracts
{
    /// <summary>
    /// Defines interface of the exchange rate service, that contains exchange rates loading from the source.
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Loads exchange rates by a given request
        /// </summary>
        /// <param name="loadCurrenciesRequest">The request.</param>
        /// <returns>Raw data splitted by line separator.</returns>
        LoadExchangeRatesResponse LoadExchangeRates(LoadExchangeRatesRequest loadCurrenciesRequest);
    }
}
