using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.ClientFactories
{
    public interface ICnbApiClientFactory : IExternalApiClientFactory
    {
        ICnbApiClient CreateCnbApiClient();
    }
}
