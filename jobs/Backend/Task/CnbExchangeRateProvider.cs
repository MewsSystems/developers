using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{

    public class CnbExchangeRateProvider : ExchangeRateProviderBase
    {
        public CnbExchangeRateProvider(IExchangeRateProviderConfiguration config) : base(config) { }

        protected override async Task<T> FetchRawDataAsync<T>()
        {
            if (typeof(T) != typeof(CnbApiResponse))
                throw new NotSupportedException($"Type {typeof(T)} is not supported by {nameof(CnbExchangeRateProvider)}");
            var result = await HttpClient.GetFromJsonAsync<CnbApiResponse>(_apiUrl);
            return (T)(object)result;
        }

        protected override IEnumerable<ExchangeRate> MapToExchangeRates<T>(T rawData, IEnumerable<Currency> currencies)
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
    }
}
