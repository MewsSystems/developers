using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ExchangeRateUpdater.ExchangeRateApi
{
    public abstract class ExchangeRateProviderBase : IExchangeRateProvider
    {
        protected readonly HttpClient HttpClient;
        protected readonly string _apiUrl;
        protected readonly Currency _baseCurrency;
        protected static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        protected ExchangeRateProviderBase(
            IExchangeRateProviderConfiguration config,
            HttpClient httpClient = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            _apiUrl = config.Url;
            _baseCurrency = new Currency(config.BaseCurrency);
            HttpClient = httpClient ?? new HttpClient();
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync<T>(IEnumerable<Currency> currencies)
        {
            try
            {
                var response = await FetchRawDataAsync<T>();
                return MapToExchangeRates(response, currencies);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error in GetExchangeRatesAsync");
                throw;
            }
        }

        protected abstract Task<T> FetchRawDataAsync<T>();
        protected abstract IEnumerable<ExchangeRate> MapToExchangeRates<T>(T rawData, IEnumerable<Currency> currencies);
    }
}
