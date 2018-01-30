using AngleSharp;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankProvider : ISpecificExchangeRateProvider
    {
        private const string _ProviderName = "Czech National Bank";
        private const string _BaseCurrencyCode = "CZK";
        private const string _BaseUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.jsp";

        private const string QuerySelector= "table.kurzy_tisk tr";
        private const string QeuryLocName = "td";

        private Currency baseCurrency = new Currency(_BaseCurrencyCode);

        public CzechNationalBankProvider()
        {
            if (Currency.IsNullOrEmpty(BaseCurrency) || string.IsNullOrEmpty(BaseURL) || string.IsNullOrEmpty(ProviderName)) throw new Exception(Res.InternalErrorProviderCZK);
        }

        /// <summary>
        /// Base currency. It can't be changed.
        /// </summary>
        public Currency BaseCurrency
        {
            get
            {
                return (baseCurrency);
            }
        }

        /// <summary>
        /// Base URL. This URL is source of data in this provider.
        /// </summary>
        public string BaseURL
        {
            get
            {
                return (_BaseUrl);
            }
        }

        /// <summary>
        /// Provider name
        /// </summary>
        public string ProviderName
        {
            get
            {
                return (_ProviderName);
            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// <param name="currencies">Currencies for output exchange rate. It can't be empty or null</param>
        /// <returns>Returns exchange rate step by step.</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || currencies.Count() <= 0) throw new Exception(Res.CurrenciesShouldBeSetForGettingER);
            string html = GetSourceData();
            if (string.IsNullOrEmpty(html)) { throw new Exception(Res.SourceDataIsEmpty); };
            var exRates = GetExchangeRatesFromSource(BaseCurrency, html);
            if (exRates == null || exRates.Count() <= 0) yield break;
            foreach (var targetCurrency in currencies)
            {
                if (Currency.IsNullOrEmpty(targetCurrency)) throw new Exception(Res.EmptyCurrencyCode);//make exception if empty currency
                foreach (var exRate in exRates)
                {
                    if (Currency.IsNullOrEmpty(exRate.TargetCurrency)) throw new Exception(Res.EmptyCurrencyCode);//make exception if empty currency
                    if (exRate.TargetCurrency.IsEqual(targetCurrency)) //if supports then add
                    {
                        yield return (new ExchangeRate(BaseCurrency, exRate.TargetCurrency, exRate.Value));
                    }
                }
            }
        }

        private string GetSourceData()
        {
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(BaseURL);
            }
            return (html);
        }

        private ExchangeRateList GetExchangeRatesFromSource(Currency sourceCurrency, string sourceData)
        {
            if (Currency.IsNullOrEmpty(sourceCurrency)) throw new Exception(Res.CurrenciesShouldBeSetForGettingER);
            var exRates = new ExchangeRateList();
            var parser = new HtmlParser(Configuration.Default.WithCss());
            var document = parser.Parse(sourceData);
            if (document == null) throw new Exception(Res.ParsingError);
            var col = document.QuerySelectorAll(QuerySelector);
            if (col == null) throw new Exception(Res.ParsingError);
            foreach (var htmlEl in col)
            {
                try
                {
                    if (htmlEl == null || htmlEl.Children.Count() != 5) continue;
                    var subCol = htmlEl.Children.Where(n => string.Compare(n.LocalName, QeuryLocName, true) == 0);
                    if (subCol == null) continue;
                    var tmpCol = subCol.ToList();
                    if ((tmpCol == null) || (tmpCol.Count() != 5)) continue;
                    if (tmpCol[3] == null || string.IsNullOrEmpty(tmpCol[3].InnerHtml)) throw new Exception(Res.ParsingError);
                    if (tmpCol[4] == null || string.IsNullOrEmpty(tmpCol[4].InnerHtml)) throw new Exception(Res.ParsingError);
                    Currency cur; decimal rate = 0;
                    try
                    {
                        cur = new Currency(tmpCol[3].InnerHtml);
                    }
                    catch { throw new Exception(Res.ParsingError); }
                    if (!decimal.TryParse(tmpCol[4].InnerHtml, out rate)) throw new Exception(Res.ParsingError);
                    exRates.Add(new ExchangeRate(sourceCurrency, cur, rate));
                }
                catch { throw; }//no ignore exceptions
            }
            return (exRates);
        }
    }
}
