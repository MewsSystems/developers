using ExchangeRateUpdater.Application.Abstractions;
using ExchangeRateUpdater.Application.Mappings;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Infrastucture.Data.API.Abstractions;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Application.Services
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IExternalAPIService _externalAPI;
        private readonly IConfiguration _configuration;

        public ExchangeRateProviderService(IExternalAPIService externalAPI, IConfiguration configuration)
        {
            _externalAPI = externalAPI;
            _configuration = configuration;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<ExchangeRateModel> GetExchangeRates(IEnumerable<CurrencyModel> request)
        {
            var sourceCurrency = _configuration["ExternalApiSettings:SourceCurrency"];

            var currencies = CurrencyMappings.Map(request);

            var response = await _externalAPI.GetFromExternalApi();

            var exchangeRates = ExchangeRateMappings.Map(response, sourceCurrency ?? "CZK");

            var RatesIntersect = exchangeRates
                       .Join(currencies,
                       e => e.TargetCurrency.Code,
                       c => c.Code,
                       (e, c) => e).ToList();

            var exchangeRateModel = ExchangeRateMappings.Map(RatesIntersect);

            return exchangeRateModel;
        }
    }
}
