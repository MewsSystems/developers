using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater.Caching
{
    public interface IExchangeRateCache
    {
        void Set(ExchangeRate[] exchangeRates);
        bool TryGetValue(out ExchangeRate[] value);
    }
}