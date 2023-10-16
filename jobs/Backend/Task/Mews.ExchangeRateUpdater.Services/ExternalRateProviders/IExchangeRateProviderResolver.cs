namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders
{
    public interface IExchangeRateProviderResolver
    {
        IExchangeRateProvider GetExchangeRateProvider();
    }
}
