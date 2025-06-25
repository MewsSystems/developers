using ExchangeRateUpdater.ExchangeRateApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb
{
    public class ExchangeRateProvider : ExchangeRateProviderBase
    {
        protected static new readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ExchangeRateProvider(
            IExchangeRateProviderConfiguration config,
            HttpClient httpClient = null)
            : base(config, httpClient) { }

        protected override async Task<CnbApiResponse> FetchRawDataAsync<CnbApiResponse>()
        {
            try
            {
                var result = await HttpClient.GetFromJsonAsync<CnbApiResponse>(_apiUrl);
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
                var apiResponse = rawData as CnbApiResponse;
                var rates = new List<ExchangeRate>();
                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);
                if (apiResponse?.Rates == null)
                { 
                    return rates; 
                }
                foreach (var rate in apiResponse.Rates)
                {
                    if (!currencyCodes.Contains(rate.CurrencyCode))
                    {
                        continue;
                    }
                    var currency = new Currency(rate.CurrencyCode);
                    int amount = rate.Amount;
                    rates.Add(new ExchangeRate(currency, _baseCurrency, rate.Rate / amount));
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
