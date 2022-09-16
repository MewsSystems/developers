using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace ExchangeRateProvider.Cache
{
    public interface ICache<TKey, TItem>
        where TItem : class
        where TKey : class
    {
        public Dictionary<Currency, ExchangeRate> Index { get; }

    }
}
