using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private string _data;
        private List<ExchangeRate> _exchangeRates;
        private Currency _sourceCurrency;
        public ExchangeRateProvider(string data)
        {
            _data = data;
            _exchangeRates = new List<ExchangeRate>();
            _sourceCurrency = new Currency("CZK");
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            ConvertStringToExchangeRate(currencies);
            return _exchangeRates;
        }

        private void ConvertStringToExchangeRate(IEnumerable<Currency> currencies)
        {
            var delims = new[] {'\n'};
            var exchangeData = _data.Split(delims,
                StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();

            foreach (var singleLine in exchangeData)
            {
                var info = singleLine.Split('|');
                var code = info[3];
                if (!currencies.Any(currency => currency.Code == info[3])) continue;
                var amount = Convert.ToInt16(info[2]);
                var rate = float.Parse(info[4]);
                
                var exchangeRate = new ExchangeRate(_sourceCurrency, new Currency(code), (decimal) (amount / rate));
                _exchangeRates.Add(exchangeRate);
            }
        }
    }
}