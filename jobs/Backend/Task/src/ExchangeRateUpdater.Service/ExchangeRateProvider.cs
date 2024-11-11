using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Ack;
using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Helpers;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Model.Cnb.Rq;
using ExchangeRateUpdater.Domain.Model.Cnb.Rs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Service
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientService httpClientService;
        private readonly CnbApiConfig cnbApiConfig;
        private readonly ILogger<ExchangeRateProvider> logger;

        public ExchangeRateProvider(IHttpClientService httpClientService,
                                   IOptions<CnbApiConfig> cnbApiConfig,
                                   ILogger<ExchangeRateProvider> logger)
        {
            this.httpClientService = httpClientService;
            this.cnbApiConfig = cnbApiConfig.Value;
            this.logger = logger;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IReadOnlyCollection<Currency> currencies)
        {
            if (!IsValidCurrencyRequest(currencies))
            {
                logger.LogWarning("Not enough currencies has been found on the request.");
                return Enumerable.Empty<ExchangeRate>();
            }

            var ackAvailableRates = await FetchSourceData();

            if (!IsResponseValid(ackAvailableRates))
            {
                logger.LogWarning("No exchange rates retrieved from the source.");
                return Enumerable.Empty<ExchangeRate>();
            }

            var rates = ackAvailableRates.Entity.Rates.ToDictionary(x => x.CurrencyCode);

            return currencies
                .DistinctBy(x => x.Code)
                .Where(currency => rates.ContainsKey(currency.Code))
                .Select(currency =>
                {
                    var rateTarget = rates[currency.Code];
                    return new ExchangeRate(
                        currency,
                        new Currency(cnbApiConfig.LocalCurrencyIsoCode),
                        rateTarget.Amount,
                        rateTarget.Rate
                    );
                });
        }

        private bool IsValidCurrencyRequest(IEnumerable<Currency> currencies)
        {
            return currencies != null && 
                   currencies.Count() > 1 &&
                   currencies.Any(x => x.Code == cnbApiConfig.LocalCurrencyIsoCode);

        }

        private bool IsResponseValid(AckEntity<CnbExchangeRatesRsModel> ackAvailableRates) 
        {
            return ackAvailableRates?.Entity?.Rates != null &&
                   ackAvailableRates.Success &&
                   ackAvailableRates.Entity.Rates.Length > 0;
        }

        private async Task<AckEntity<CnbExchangeRatesRsModel>> FetchSourceData()
        {
            var model = new CnbExchangeRatesRqModel(cnbApiConfig.PreferredLanguage);
            var uri = UrlHelper.GetUriFromModelWithParams(cnbApiConfig.ExchangeRateApiUrl, model);

            return await httpClientService.GetAsync<CnbExchangeRatesRsModel>(cnbApiConfig.ClientName, uri);
        }

    }
}
