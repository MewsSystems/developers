using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using NLog;

namespace ExchangeRateUpdater.ExchangeRateApi
{
    public class ExchangeRateApiProvider : ExchangeRateProviderBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ExchangeRateApiProvider(IExchangeRateProviderConfiguration config) : base(config) { }

        protected override async Task<ExchangeRateApiResponse> FetchRawDataAsync<ExchangeRateApiResponse>()
        {
            try
            {
                var result = await HttpClient.GetFromJsonAsync<ExchangeRateApiResponse>(_apiUrl);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching raw data in FetchRawDataAsync");
                throw;
            }
        }

        protected override IEnumerable<ExchangeRate> MapToExchangeRates<T>(T rawData, IEnumerable<Currency> currencies)
        {
            try
            {
                var response = rawData as ExchangeRateApiResponse;
                var rates = new List<ExchangeRate>();
                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

                if (response?.ConversionRates == null)
                    return rates;

                foreach (var code in currencyCodes)
                {
                    decimal rate;
                    if (response.ConversionRates.TryGetValue(code, out rate))
                    {
                        var currency = new Currency(code);
                        rates.Add(new ExchangeRate(currency, _baseCurrency, rate));
                    }
                }
                return rates;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error mapping to exchange rates in MapToExchangeRates");
                throw;
            }
        }
    }
}
