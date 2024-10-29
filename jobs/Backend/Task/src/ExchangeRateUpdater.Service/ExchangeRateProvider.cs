using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Model.Cnb.Rq;
using ExchangeRateUpdater.Domain.Model.Cnb.Rs;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Service
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly CnbApiConfig cnbApiConfig;
        private readonly IHttpClientService pollyClient;

        public ExchangeRateProvider(IHttpClientService pollyClient,
                                   IOptions<CnbApiConfig> cnbApiConfig)
        {
            this.cnbApiConfig = cnbApiConfig.Value;
            this.pollyClient = pollyClient;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies.Count() < 1 || !currencies.Any(x=> x.Code == cnbApiConfig.LocalCurrencyIsoCode))
                return [];

            var availableRates = await pollyClient.GetAsync<CnbExchangeRatesRsModel, CnbExchangeRatesRqModel>(
                cnbApiConfig.ExchangeRateApiUrl,
                new CnbExchangeRatesRqModel(cnbApiConfig.PreferredLanguage));

            if (availableRates == null)
                return [];

            return currencies
                .DistinctBy(x => x.Code)
                .Where(currency => availableRates.Rates.Any(x => x.CurrencyCode == currency.Code))
                .Select(currency =>
                {
                    var rateTarget = availableRates.Rates.First(x => x.CurrencyCode == currency.Code);
                    return new ExchangeRate(
                        new Currency(cnbApiConfig.LocalCurrencyIsoCode),
                        currency,
                        rateTarget.Amount,
                        rateTarget.Rate
                    );
                });
        }
    
        
    }
}
