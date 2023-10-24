using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Repository.Abstract;
using ExchangeRateUpdater.Service.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Service
{
	public class ExchangeRateService : IExchangeRateService
    {
        private readonly ICzechNationalBankRepository _czechNationalBankRepository;

        public ExchangeRateService(ICzechNationalBankRepository czechNationalBankRepository)
        {
			_czechNationalBankRepository = czechNationalBankRepository;
		}
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            var externalRates = await _czechNationalBankRepository.FetchCurrencyRates();

            foreach (var currency in currencies)
            {
                var externalRate = externalRates.FirstOrDefault(x => x.CurrencyCode == currency.Code);
                if (externalRate == null) continue;
                var rate = new ExchangeRate(new Currency(externalRate.CurrencyCode), new Currency("CZK"), externalRate.Rate);
                exchangeRates.Add(rate);
            }

            return exchangeRates;
        }
    }
}
