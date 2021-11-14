using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public abstract class CnbExchangeRatesSource : IExchangeRatesSource
    {
        protected IDateTimeProvider DateTimeProvider { get; set; }

        protected DateTime DownloadTime { get; set; }

        private Dictionary<ExchangePair, ExchangeRate> ExchangeRateCache { get; set; }

        private IWebClientFactory WebClientFactory { get; set; }

        protected CnbExchangeRatesSource()
        {
            // here I should use some inject
            DateTimeProvider = new DateTimeProvider();
            WebClientFactory = new WebClientFactory();
        }

        public ExchangeRate Get(ExchangePair currenyPair)
        {
            if (IsOudated())
                LoadExchangeRates();

            if (ExchangeRateCache.TryGetValue(currenyPair, out var exchangeRate))
                return exchangeRate;

            return null;
        }

        protected abstract string GetUrl(DateTime day);

        protected abstract bool IsOudated();

        private static Dictionary<ExchangePair, ExchangeRate> ParseExchangeRates(string exchangeRatesCsv)
        {
            return exchangeRatesCsv.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Skip(2)
                .Select(x =>
                {
                    var exchangeRate = x.Split('|');

                    if (exchangeRate.Length < 5)
                        throw new IndexOutOfRangeException();

                    if (!int.TryParse(exchangeRate[2], out var targetCurrencyAmount))
                        throw new InvalidCastException();

                    var targetCurrencyCode = exchangeRate[3];

                    if (!decimal.TryParse(exchangeRate[4], out var targetCurrencyRate))
                        throw new InvalidCastException();

                    if (targetCurrencyAmount == 0)
                        throw new DivideByZeroException();

                    return new ExchangeRate(new Currency(targetCurrencyCode), new Currency("CZK"), targetCurrencyRate / targetCurrencyAmount);
                })
                .ToDictionary(x => new ExchangePair(x.SourceCurrency, x.TargetCurrency));
        }

        private string DownloadString(string url)
        {
            using (var client = WebClientFactory.Create())
            {
                return client.DownloadString(url);
            }
        }

        private void LoadExchangeRates()
        {
            var downloadTime = DateTimeProvider.Now;

            var url = GetUrl(downloadTime);
            var exchangeRatesCsv = DownloadString(url);
            var exchangeRates = ParseExchangeRates(exchangeRatesCsv);

            ExchangeRateCache = exchangeRates;
            DownloadTime = downloadTime;
        }
    }
}