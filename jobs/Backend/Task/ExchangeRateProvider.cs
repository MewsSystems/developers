using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly CnbSettings _settings;
        private readonly Currency _targetCurrency;
        private readonly ICnbClient _cnbClient;

        public ExchangeRateProvider(CnbSettings settings, ICnbClient cnbClient)
        {
            _settings = settings;
            _targetCurrency = new Currency(_settings.TargetCurrencyCode);
            _cnbClient = cnbClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var requestedCurrencies = currencies.ToDictionary(c => c.Code, c => c, StringComparer.OrdinalIgnoreCase);

            if (requestedCurrencies.Count == 0)
            {
                return [];

            }

            var (success, rawData) = _cnbClient.GetRawDailyExchanges();
            if (!success)
            {
                return [];
            }

            var rates = ParseData(rawData, requestedCurrencies);
            return rates;
        }

        private IEnumerable<ExchangeRate> ParseData(string rawData, Dictionary<string, Currency> requestedCurrencies)
        {
            var rates = new List<ExchangeRate>();
            var lines = rawData.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(2))
            {
                var cols = line.Split('|');
                if (cols.Length < 5) continue;

                var code = cols[3].Trim();
                if (requestedCurrencies.TryGetValue(code, out var sourceCurrency))
                {
                    if (int.TryParse(cols[2].Trim(), out int amount) && amount > 0 &&
                        decimal.TryParse(cols[4].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rateValue))
                    {
                        rates.Add(new ExchangeRate(sourceCurrency, _targetCurrency, rateValue / amount));
                    }
                }
            }
            return rates;
        }
    }
}
