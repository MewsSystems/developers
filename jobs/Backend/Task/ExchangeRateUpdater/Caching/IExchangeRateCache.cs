using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater.Caching
{
    public interface IExchangeRateCache
    {
        ExchangeRate[]? GetValue();
        void Set(ExchangeRate[] exchangeRates);
    }
}