using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.CzechNationalBank.HttpClient;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        readonly CzechNationalBankApiClient _nationalBankApiClient;

        public ExchangeRateProvider()
        {
            _nationalBankApiClient = new CzechNationalBankApiClient();
        }
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currentExchangeRates = _nationalBankApiClient.GetExchangeRatesAsync(DateTime.UtcNow).Result;

            var currencyCodes = currencies.Select(c => c.Code);

            var targetCurrency = new Currency("CZK");
            
            var filteredExchangeRates = currentExchangeRates
                .Where(exchangeRateDto => exchangeRateDto.Currency is not null &&
                                          exchangeRateDto.Rate is not null &&
                                          exchangeRateDto.Amount is not null &&
                                          currencyCodes.Contains(exchangeRateDto.Currency.Code))
                .Select(exchangeRateDto => new ExchangeRate(
                    exchangeRateDto.Currency,
                    targetCurrency,
                    (decimal)exchangeRateDto.Rate / (decimal)exchangeRateDto.Amount));

            return filteredExchangeRates;
        }
    }
}
