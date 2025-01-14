namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies
{
    /// <summary>
    /// Defines a common interface for all concrete strategies
    /// </summary>
    public interface IExchangeRateProviderStrategy
    {
        public bool CanHandle(string providerCode);

        public Task<IEnumerable<GetExchangeRatesQueryResponse>> GetExchangeRatesAsync(GetExchangeRatesQuery request, CancellationToken cancellationToken);
    }
}
