
namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies
{
    /// <summary>
    /// Base class for the implementation of specific strategies.
    /// </summary>
    public abstract class ExchangeRateProviderStrategyBase : IExchangeRateProviderStrategy
    {
        protected abstract string ProviderCode { get; }

        public bool CanHandle(string providerCode)
        {
            return ProviderCode == providerCode;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<IEnumerable<GetExchangeRatesQueryResponse>> GetExchangeRatesAsync(GetExchangeRatesQuery request, CancellationToken cancellationToken);
    }
}
