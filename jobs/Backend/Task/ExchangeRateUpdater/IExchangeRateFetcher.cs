using ExchangeRateUpdater.Dto;

namespace ExchangeRateUpdater;

public interface IExchangeRateFetcher
{
    Task<ExchangeRatesBo?> FetchCurrentAsync();
}