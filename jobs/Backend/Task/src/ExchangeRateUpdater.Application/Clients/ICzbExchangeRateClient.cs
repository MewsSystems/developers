using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.ExchangeRates;

namespace ExchangeRateUpdater.Application.Clients;

public interface ICzbExchangeRateClient
{
    public Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRate(string currency, DateTime? dateTime = null);
}
