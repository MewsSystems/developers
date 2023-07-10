using ExchangeRateUpdater.CnbProvider.Abstractions;
using ExchangeRateUpdater.CnbProvider.CnbClientResponses;
using ExchangeRateUpdater.CnbProvider.Enums;
using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProvider
{
    public class CnbRateProvider : ICnbRateProvider
    {
        private readonly CnbLanguageEnum defaultLanguage = CnbLanguageEnum.EN;
        private const string targetCurrency = "CZK";

        private ICnbRateProviderClient _cnbRateProviderClient;

        public CnbRateProvider(ICnbRateProviderClient cnbRateProviderClient)
        {
            _cnbRateProviderClient = cnbRateProviderClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetRatesByDateAsync(DateTime date)
        {
            var cbnRates = new[]
            { 
                await _cnbRateProviderClient.GetRatesByDateAsync(new CnbRateRequestVO(date, defaultLanguage).UrlDaily()), 
                await _cnbRateProviderClient.GetRatesByDateAsync(new CnbRateRequestVO(date, defaultLanguage).UrlMonthly()),
            }
            .SelectMany(ExchangeRateValuesMapper);

            return cbnRates;
        }


        private IEnumerable<ExchangeRate> ExchangeRateValuesMapper(IEnumerable<CnbRateResponseDto> rates)
        {
            return rates.Where(w => w.Amount > 0).Select(s => new ExchangeRate(new Currency(s.CurrencyCode), new Currency(targetCurrency), s.Rate / s.Amount));
        }
    }
}
