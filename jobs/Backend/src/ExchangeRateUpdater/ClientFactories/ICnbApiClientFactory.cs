using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.ClientFactories
{
    public interface ICnbApiClientFactory
    {
        ICnbApiClient CreateCnbApiClient();
    }
}
