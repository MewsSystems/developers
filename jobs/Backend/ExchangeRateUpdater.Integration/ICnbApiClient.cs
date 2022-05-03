namespace ExchangeRateUpdater.Integration
{
    /// <summary>
    /// Interface for CNB Api Client.
    /// 
    /// Currently only for testing purposes - should be abstraction created in this project and implementation under CNB project.
    /// </summary>
    public interface ICnbApiClient
    {
        /// <summary>
        /// Gets exchange rates for basic currencies.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CnbExchangeRate>> GetBasicRatesAsync();

        /// <summary>
        /// Gets exchange rates for other currencies.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CnbExchangeRate>> GetOtherCurrenciesRatesAsync();
    }
}
