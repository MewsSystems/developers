using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private UriBuilder FeedUri = new UriBuilder("http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt");
        private List<ExchangeRate> Rates { get; set; }

        public ExchangeRateProvider(DateTime date)
        {
            FeedUri.Query = String.Format("date={0}", date.ToString("dd.MM.yyyy"));
            UpdateExhcangeRates();
        }

        public void UpdateExhcangeRates()
        {
            Rates = new List<ExchangeRate>();
            using (WebClient client = new WebClient())
            {
                string txtFeed = client.DownloadString(FeedUri.Uri);
                ParseFeed(txtFeed);
            }
        }

        private void ParseFeed(string txtFeed)
        {
            // Row follows pattern country|currency name|multiplier|code|value
            List<string> lines = txtFeed.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.RemoveRange(0, 2);
            foreach (string line in lines)
            {
                string[] splitRow = line.Split('|');
                if (splitRow.Count() < 5)
                    throw new Exception("Data source doesn't follow specified pattern");

                // CNB has only currency/CZK exchange rates available in TXT feed
                Currency sourceCurrency = new Currency(splitRow[3]);
                Currency targetCurrency = new Currency("CZK");
 
                decimal value = 0;
                bool decSuccess = decimal.TryParse(splitRow[4], out value);
                if (!decSuccess)
                    throw new Exception("Value in feed is not a number");


                int multiplier = 1;
                bool intSuccess = int.TryParse(splitRow[2], out multiplier);
                if (!decSuccess)
                    throw new Exception("Multiplier in feed is not a number");

                ExchangeRate rate = new ExchangeRate(sourceCurrency, targetCurrency, value/multiplier);
                Rates.Add(rate);
            }
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // Will always return all as CZK is present in every rate as CNB has only currency/CZK rates in TXT feed
            List<string> requestedCurrencies = currencies.Select(x => x.Code).ToList();
            return Rates.Where(rate => requestedCurrencies.IndexOf(rate.SourceCurrency.Code) > -1 || requestedCurrencies.IndexOf(rate.TargetCurrency.Code) > -1).ToList();
        }
    }
}
