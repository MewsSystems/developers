using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mews.ExchangeRateUpdater.Domain.Entities;
using Mews.ExchangeRateUpdater.Domain.Interfaces;

namespace Mews.ExchangeRateUpdater.Domain.Services
{
    public class CNBExchangeRateProvider : IExchangeRateProvider
    {
        private readonly CNB.CnbClient _cnbClient;
        private readonly IMapper _mapper;

        public CNBExchangeRateProvider(CNB.CnbClient cnbClient,
            IMapper mapper)
        {
            _cnbClient = cnbClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencyCodes, DateTime? date)
        {
            if (currencyCodes == null || !currencyCodes.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var allExchangRates = await GetAllExchangeRatesFromCNB(date).ConfigureAwait(false);

            var exchangeRatesForRequestedCurrencies = FilterExchangeRates(allExchangRates, currencyCodes);

            var mappedExchangeRates = _mapper.Map<IEnumerable<ExchangeRate>>(exchangeRatesForRequestedCurrencies);

            return mappedExchangeRates;
        }

        private async Task<IReadOnlyCollection<CNB.ExchangeRate>> GetAllExchangeRatesFromCNB(DateTime? date)
        {
            return await _cnbClient.ExchangeRateAll(date).ConfigureAwait(false);
        }

        private IEnumerable<CNB.ExchangeRate> FilterExchangeRates(IReadOnlyCollection<CNB.ExchangeRate> allExchangRates, IEnumerable<string> currencyCodes)
        {
            if (allExchangRates == null || !allExchangRates.Any())
            {
                return Enumerable.Empty<CNB.ExchangeRate>();
            }

            return allExchangRates
                .Where(exchangeRate =>
                    currencyCodes
                    .Any(currencyCode =>
                        string.Equals(currencyCode, exchangeRate.Code, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
