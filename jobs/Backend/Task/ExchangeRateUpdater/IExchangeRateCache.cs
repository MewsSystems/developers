using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateCache
    {
        void Set(ExchangeRate[] exchangeRates);
        bool TryGetValue(out ExchangeRate[] value);
    }
}