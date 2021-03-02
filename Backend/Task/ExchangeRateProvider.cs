using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRateList = new List<ExchangeRate>();
            Uri url = new Uri(ConfigurationManager.AppSettings["ExchangeRatesURLToCrawl"]);
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var rows = document.DocumentNode.SelectNodes("//table[@class='currency-table']//tr");
            if (rows != null && rows.Count > 0)
            {
                rows.RemoveAt(0); //remove header
                foreach (var row in rows)
                {
                    var cols = row.SelectNodes("td");
                    if (CheckColumnValidity(cols))
                    {
                        var isCurrencyExists = currencies.Any(c => cols[3].InnerText.Contains(c.Code));
                        if (isCurrencyExists)
                        {
                            string sourceCurrency = cols[3].InnerText;
                            decimal currencyValue;

                            decimal tmpAmount;
                            decimal? amount = decimal.TryParse((string)cols[2].InnerText, out tmpAmount) ?
                                              tmpAmount : (decimal?)null;

                            decimal tmpValue;
                            decimal? value = decimal.TryParse((string)cols[4].InnerText, out tmpValue) ?
                                              tmpValue : (decimal?)null;

                            if (amount != null)
                            {
                                try
                                {
                                    currencyValue = (Decimal.Divide(tmpValue, (decimal)amount));
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            else
                            {
                                currencyValue = value == null ? 0 : (decimal)value;
                            }
                            exchangeRateList.Add(item: new ExchangeRate(new Currency(sourceCurrency), new Currency(ConfigurationManager.AppSettings["TargetCurrency"]), currencyValue));
                        }
                    }
                }
            }

            return exchangeRateList;
        }

        /// <summary>Checks validity of columns./// </summary>
        /// <param name="cols">Columns of currency table</param>
        /// <returns></returns>
        private bool CheckColumnValidity(HtmlNodeCollection cols)
        {
            if (cols != null && cols.Count >= 5)
            {
                for (int i = 2; i < cols.Count; i++)
                {
                    if (String.IsNullOrWhiteSpace(cols[i].InnerText))
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
