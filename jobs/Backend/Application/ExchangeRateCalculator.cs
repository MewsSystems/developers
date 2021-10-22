using Domain;
using Infrastructure.Entities;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public class ExchangeRateCalculator
    {
        private readonly IExchangeRateService _service;
        private readonly Dictionary<string, GenericRate> _sourceRates = new();
        private bool _isInitialized;
        public ExchangeRateCalculator(IExchangeRateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new();

            await GetRatesAsync();

            foreach (var currency in currencies)
            {
                if (!IsCurrencyPresent(currency.Code))
                    continue;

                var exchangeRate = CalculateExchangeRate(currency);
                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }

        private async Task GetRatesAsync()
        {
            if (_isInitialized)
                return;

            var sourceRates = await _service.GetExchangeRatesAsync();

            PopulateDictionary(sourceRates.ToList());

            _isInitialized = true;
        }

        private bool IsCurrencyPresent(string code)
        {
            return _sourceRates.ContainsKey(code);
        }

        private void PopulateDictionary(List<GenericRate> sourceRates)
        {
            foreach (var rate in sourceRates)
            {
                _sourceRates[rate.Code] = rate;
            }
        }

        private ExchangeRate CalculateExchangeRate(Currency currency)
        {
            var targetCurrency = _sourceRates[currency.Code];
            var currentRate = Convert.ToDecimal(targetCurrency.CurrentRate);
            var amount = Convert.ToDecimal(targetCurrency.Amount);
            var exchangeRate = amount / currentRate;

            return new ExchangeRate(
                new Currency("CZK"),
                new Currency(targetCurrency.Code),
                exchangeRate
            );
        }

    }
}
