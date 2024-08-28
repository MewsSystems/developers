using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;

namespace ExchangeRateUpdater.Infrastructure.Cache.Abstract;

public interface ICacheAccessor<T>
{
    Task<IEnumerable<CzechNationalBankExchangeRate>?> GetAsync();
    Task SetAsync(T value);
}
