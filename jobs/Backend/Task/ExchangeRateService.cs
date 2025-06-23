using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateService : IExchangeRateService
    {
        private Dictionary<string, List<ExchangeRate>> _cachedExchangeRates = new Dictionary<string, List<ExchangeRate>>();
        private IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateService(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                return new List<ExchangeRate>();
            }

            var today = DateAndTime.Now;
            var dayAsKey = today.ToString("MM.dd.yyyy");

            var exchangeRates = new List<ExchangeRate>();
            if (_cachedExchangeRates.TryGetValue(dayAsKey, out List<ExchangeRate> cachedRates))
            {
                exchangeRates = cachedRates;
            }
            else
            {
                var exchangeRatesResult = "";
                try
                {
                    exchangeRatesResult = await _exchangeRateProvider.GetExchangeRates(dayAsKey);
                }
                catch (Exception ex)
                {
                    return new List<ExchangeRate>();
                }
                
                if (String.IsNullOrEmpty(exchangeRatesResult))
                {
                    return new List<ExchangeRate>();
                }
                var exchangeRatesFinal = GetExchangeRatesFromText(exchangeRatesResult);
                _cachedExchangeRates[dayAsKey] = exchangeRatesFinal;
                exchangeRates = exchangeRatesFinal;
            }

            return GetMatchingRates(exchangeRates, currencies);
        }

        private List<ExchangeRate> GetExchangeRatesFromText(string exchangeRatesSource)
        {
            string[] rows = exchangeRatesSource.Split('\n');
            var results = new List<ExchangeRate>();
            for (int i = 2; i < rows.Count() - 1; i++)
            {
                var values = rows[i].Split('|');
                var exchangeRate = new ExchangeRate(new Currency(values[3]), new Currency("CZK"), Decimal.Parse(values[4]) / Decimal.Parse(values[2]));
                var inverseExchangeRate = new ExchangeRate(new Currency("CZK"), new Currency(values[3]), Decimal.Parse(values[2]) / Decimal.Parse(values[4]));
                results.Add(exchangeRate);
            }
            return results;
        }

        private IEnumerable<ExchangeRate> GetMatchingRates(List<ExchangeRate> lookupList, IEnumerable<Currency> requestedCurrencies)
        {
            // easy to change below line if the match should be made on both source and target currency
            return lookupList.Where(curr => requestedCurrencies.FirstOrDefault(req => req.Code == curr.SourceCurrency.Code) != null);
        }
    }
}
