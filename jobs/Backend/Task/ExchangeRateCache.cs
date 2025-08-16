using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateCache
    {
        Task<List<ExchangeRateRecord>> GetCachedValuesAsync();
    }

    public class ExchangeRateCache : IExchangeRateCache
    {
        private readonly ICnbApiWrapper _cnbApi;

        public ExchangeRateCache(ICnbApiWrapper cnbApi)
        {
            _cnbApi = cnbApi;
        }

        public async Task<List<ExchangeRateRecord>> GetCachedValuesAsync()
        {
            return await _cnbApi.GetExchangeRatesAsync();
        }

    }
}
