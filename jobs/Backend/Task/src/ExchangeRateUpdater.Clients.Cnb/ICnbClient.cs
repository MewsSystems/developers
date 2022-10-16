using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Clients.Cnb;

public interface ICnbClient
{
    Task<ExchangeRatesResponse> GetExchangeRatesAsync();
}