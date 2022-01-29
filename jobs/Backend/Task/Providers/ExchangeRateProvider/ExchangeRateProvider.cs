using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient;

namespace ExchangeRateUpdater.Providers.ExchangeRateProvider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        readonly ICzechNationalBankApiClient _czechNationalBankApiClient;

        public ExchangeRateProvider(ICzechNationalBankApiClient czechNationalBankApiClient)
        {
            _czechNationalBankApiClient = czechNationalBankApiClient;
        }
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currentExchangeRates = _czechNationalBankApiClient.GetExchangeRatesAsync(DateTime.UtcNow).Result;

            var currencyCodes = currencies.Select(c => c.Code);

            var targetCurrency = new Currency("CZK");
            
            var filteredExchangeRates = currentExchangeRates
                .Where(exchangeRateDto => exchangeRateDto.Currency is not null &&
                                          (exchangeRateDto.Rate is not null && exchangeRateDto.Rate > 0) && 
                                          (exchangeRateDto.Amount is not null && exchangeRateDto.Amount > 0) &&
                                          currencyCodes.Contains(exchangeRateDto.Currency.Code))
                .Select(exchangeRateDto => new ExchangeRate(
                    exchangeRateDto.Currency,
                    targetCurrency,
                    (decimal)exchangeRateDto.Rate / (decimal)exchangeRateDto.Amount));

            return filteredExchangeRates;
        }
    }
}
