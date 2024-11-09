using Cnb.Api.Client;

namespace ExchangeRateUpdater
{
    public interface ICnbApiClientFactory
    {
        ICnbApiClient CnbApiClient { get; }
    }
}