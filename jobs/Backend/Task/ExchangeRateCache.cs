using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateCache
    {
        List<ExchangeRateRecord> GetCachedValues();
    }

    public class ExchangeRateCache : IExchangeRateCache
    {
        private readonly ICnbApiWrapper _cnbApi;

        public ExchangeRateCache(ICnbApiWrapper cnbApi)
        {
            _cnbApi = cnbApi;
        }

        public List<ExchangeRateRecord> GetCachedValues()
        {
            return _cnbApi.GetExchangeRates();
        }

    }
}
