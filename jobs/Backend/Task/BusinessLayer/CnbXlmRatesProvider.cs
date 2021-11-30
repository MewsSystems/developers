using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.BusinessLayer
{
    public class CnbXlmRatesProvider : ExchangeRateProvider
    {
        private readonly ICnbXmlSource source;

        private const string DefaultTargetCurrency = "CZK";

        public CnbXlmRatesProvider(ICnbXmlSource source)
        {
            this.source = source;
        }


        public override IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currenciesDictionary = currencies.ToDictionary(c => c.Code);
            if (!currenciesDictionary.ContainsKey(DefaultTargetCurrency))
            {
                // We could return an empty rates collection here if needed.
                throw new ArgumentException(
                    "Currencies list doesn't contain 'CZK' currency which is default target currency for CNB");
            }

            var targetCurrency = new Currency(DefaultTargetCurrency);
            //Not async not to change ExchangeProvider signature
            var rates = source.GetRates().Result;
            return rates.RatesTable.Rates
                .Where(r => currenciesDictionary.ContainsKey(r.CurrencyCode)).Select(i =>
                    new ExchangeRate(currenciesDictionary[i.CurrencyCode], targetCurrency,
                        i.Value / i.Multiplier));
        }
    }
}
