using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private static readonly Currency CzkCurrency = new Currency("CZK");
        private readonly CzechNationalBankService _czechNationalBankService;

        protected ExchangeRateProvider(CzechNationalBankService czechNationalBankService)
            => _czechNationalBankService = czechNationalBankService;
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies) 
            => _czechNationalBankService.GetRates()
                .Select(MapCnbExchangeRateIntoExchangeRate);

        private static ExchangeRate MapCnbExchangeRateIntoExchangeRate(CzechNationalBankExchangeRate cnbExchangeRate) 
            => new(new Currency(cnbExchangeRate.Code), CzkCurrency, cnbExchangeRate.Rate);
    }
}
