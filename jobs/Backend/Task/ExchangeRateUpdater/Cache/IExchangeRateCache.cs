using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cache;

public interface IExchangeRateCache
{
    public Task<IEnumerable<ExchangeRate>> GetOrCreateAsync(string key, Func<Task<IEnumerable<ExchangeRate>>> createFunc, Func<DateTime?> expirationDateCreationFunc);

    public void Remove(string key);
}