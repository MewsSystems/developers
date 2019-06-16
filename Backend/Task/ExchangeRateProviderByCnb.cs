using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderByCnb : IExchangeRateProvider
    {
        private readonly string _sourceUrl;

        public ExchangeRateProviderByCnb(string sourceUrl)
        {
            _sourceUrl = sourceUrl;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var providedExchanges = GetAllExchangeRates(_sourceUrl);

            return providedExchanges
                .Where(q => currencies.Contains(q.TargetCurrency, new CurrencyEqualityComparer()));
        }

        private IEnumerable<ExchangeRate> GetAllExchangeRates(string sourceUrl)
        {
            var rawExchangeRates = GetRawExchangeRates(sourceUrl);
            
            var currencyLines = GetCurrencyLines(rawExchangeRates);

            return BuildExchangeRates(currencyLines);
        }

        private IEnumerable<ExchangeRate> BuildExchangeRates(IEnumerable<string> currencyLines)
            => currencyLines
                .Where(q => !string.IsNullOrEmpty(q))
                .Select(BuildExchangeRateByLine);


        private ExchangeRate BuildExchangeRateByLine(string currencyLine)
        {
            if(string.IsNullOrWhiteSpace(currencyLine))
                { throw new ArgumentNullException(nameof(currencyLine)); }

            var items = currencyLine.Split('|');

            if (items.Length != 5)
                { throw new ArgumentOutOfRangeException($"expected 5 items in currency line {currencyLine}"); }


            var targetCode = items[3];
            var rate = decimal.Parse(items[4]);

            return new ExchangeRate(
                new Currency("CZK"),
                new Currency(targetCode),
                rate);
        }

        private IEnumerable<string> GetCurrencyLines(string rawExchangeRates)
        {
            return rawExchangeRates
                .Split('\n')
                .Skip(2); // bypassing DATE and HEADER
        }


        public virtual string GetRawExchangeRates(string urlAddress)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadString(urlAddress);
            }
        }
    }
}