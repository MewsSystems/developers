namespace ExchangeRateUpdater
{
    public interface IExchangeRateProviderFactory
    {
        IExchangeRateProvider GetExchangeRateProvider(ProviderName providerName);
    }
}