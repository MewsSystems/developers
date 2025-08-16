using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetCachedDataAsync(string cacheKey);
        Task SaveDataToCacheAsync(string cacheKey, string data);
    }
}
