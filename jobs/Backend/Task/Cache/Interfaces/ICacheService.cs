using System;

namespace ExchangeRateUpdater.Cache
{
    public interface ICacheService<TKey, TValue>
    {
        TValue Get(TKey key);
        void Add(TKey key, TValue value, TimeSpan duration);
    }
}
