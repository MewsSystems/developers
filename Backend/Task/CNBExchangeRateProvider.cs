using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider : AbstractExchangeRateProvider, ICurrentExchangeRateProvider
    {
        public CNBExchangeRateProvider(Currency targetCurrency)
        {
            TargetCurrency = targetCurrency;
            ExchangeRateUri = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
        }

        protected override ExchangeRateRecord ParseLine(string line)
        {
            var currencyInfo = line.Split('|');
            var isoCode = currencyInfo[3];
            var exchangeRate = decimal.Parse(currencyInfo[4], CultureInfo.InvariantCulture);
            var unit = int.Parse(currencyInfo[2]);
            return new ExchangeRateRecord(isoCode, exchangeRate, unit);
        }

        public IEnumerable<ExchangeRate> GetCurrentExchangeRates(IEnumerable<Currency> sourceCurrencies)
        {
            var allExchangeRates = ReadAllExchangeRates();

            var filteredSourceCurrencies = FindCurrenciesWithExchangeRates(sourceCurrencies, allExchangeRates);

            foreach (Currency currency in filteredSourceCurrencies)
            {
                yield return new ExchangeRate(currency, TargetCurrency, allExchangeRates.FirstOrDefault(exchangeRate => exchangeRate.IsoCode == currency.Code).ExchangeRate);
            }
        }

        private static IEnumerable<Currency> FindCurrenciesWithExchangeRates(IEnumerable<Currency> currencies, IEnumerable<ExchangeRateRecord> exchangeRates)
        {
            return currencies.Where(currency => exchangeRates.Any(exchangeRate => exchangeRate.IsoCode == currency.Code));
        }
    }
}