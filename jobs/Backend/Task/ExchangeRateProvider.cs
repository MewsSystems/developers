using CNB = Cnb.Api.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly CNB.ICnbApiClient _cnbApiClient;
        private readonly IMapper _mapper;

        public ExchangeRateProvider(
            ICnbApiClientFactory cnbApiClientFactory,
            IMapper mapper
            )
        {
            this._cnbApiClient = cnbApiClientFactory.CnbApiClient;
            this._mapper = mapper;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await this._cnbApiClient.DailyUsingGET_ExRatesDailyAsync(null, CNB.Lang.EN);

            var exchangeRates = this._mapper.Map<List<ExchangeRate>>(response.Rates);

            var filteredExchangeRates = exchangeRates
                .Where((rate) => currencies.Any((currency) => currency.Code == rate.SourceCurrency.Code));

            return filteredExchangeRates;
        }
    }
}
