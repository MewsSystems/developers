using ExchangeRate.Datalayer.Configuration;
using ExchangeRate.Datalayer.Models;
using ExchangeRate.Datalayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Providers.CzechNationalBank.Provider
{
    internal class CurrencyExchangeProvider : ICurrencyExchangeProvider
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IBaseApiSettings _apiSettings;

        public CurrencyExchangeProvider(IExchangeRateService exchangeRateService, IBaseApiSettings connectorSettings)
        {
            _exchangeRateService = exchangeRateService;
            _apiSettings = connectorSettings;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<CurrencyExchangeRate>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies)
        {
            ValidateRequiredConfigurationAndThrowOnError();

            var bankExchangeRates = await _exchangeRateService.GetExchangeRates();

            ExtractValidExchangeRates(requestedCurrencies, bankExchangeRates, out List<CurrencyExchangeRate> exchangeRates);

            return exchangeRates;
        }

        private void ExtractValidExchangeRates(IEnumerable<Currency> requestedCurrencies, IEnumerable<ProviderExchangeRate> bankExchangeRates, out List<CurrencyExchangeRate> exchangeRates)
        {
            exchangeRates = new List<CurrencyExchangeRate>();
            foreach (var requestedCurrency in requestedCurrencies)
            {
                var bankExchangeRate = bankExchangeRates.FirstOrDefault(bankExchangeRate => bankExchangeRate.ThreeLetterISOCurrencyCode == requestedCurrency.ThreeLetterISOCurrencyCode);
                if (bankExchangeRate == null)
                    continue;
                exchangeRates.Add(new CurrencyExchangeRate(new Currency(_apiSettings.SourceCurrency), new Currency(bankExchangeRate.ThreeLetterISOCurrencyCode), bankExchangeRate.Rate));
            }
        }

        private void ValidateRequiredConfigurationAndThrowOnError()
        {
            if (string.IsNullOrEmpty(_apiSettings.SourceCurrency))
            {
                throw new Exception($"{nameof(BaseApiSettings)}.{nameof(BaseApiSettings.SourceCurrency)} is missing from configuration");
            }
        }
    }
}
