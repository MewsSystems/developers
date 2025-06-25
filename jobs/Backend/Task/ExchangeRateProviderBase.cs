using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Json;

namespace ExchangeRateUpdater
{
    public abstract class ExchangeRateProviderBase : IExchangeRateProvider
    {
        protected static readonly HttpClient HttpClient = new HttpClient();
        protected readonly string _apiUrl;
        protected readonly Currency _baseCurrency;

        protected ExchangeRateProviderBase(IExchangeRateProviderConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            _apiUrl = config.Url;
            _baseCurrency = new Currency(config.BaseCurrency);
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync<T>(IEnumerable<Currency> currencies)
        {
            var response = await FetchRawDataAsync<T>();
            return MapToExchangeRates(response, currencies);
        }

        protected abstract Task<T> FetchRawDataAsync<T>();
        protected abstract IEnumerable<ExchangeRate> MapToExchangeRates<T>(T rawData, IEnumerable<Currency> currencies);
    }
}
