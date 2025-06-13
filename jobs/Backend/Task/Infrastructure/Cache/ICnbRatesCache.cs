using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Cache
{
    public interface ICnbRatesCache
    {
        Task<Dictionary<string, decimal>> GetOrCreateAsync(Func<Task<Dictionary<string, decimal>>> factory);
    }
}