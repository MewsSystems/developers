using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {

        private const string XmlApi =
            @"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        private const int NewRatesPublishHour = 14;
        private RateTable Cache { get; set; }
        private DateTime RefreshCache { get; set; } = DateTime.MinValue;

        private RateTable DownloadTable()
        {
            var now = DateTime.Now;
            if (Cache != null && (now.Date < RefreshCache.Date || now >= RefreshCache.Date && now.Hour < NewRatesPublishHour)) return Cache;

            using (var client = new WebClient())
            using (var dataStream = new MemoryStream(client.DownloadData(XmlApi)))
            {
                var xmlSerializer = new XmlSerializer(typeof(RateTable));
                Cache = (RateTable)xmlSerializer.Deserialize(dataStream);
                RefreshCache = DateTime.ParseExact(Cache.Date, "dd.MM.yyyy", null).AddDays(1);
                return Cache;
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
            var rates = DownloadTable().Rates.ToDictionary(x => x.Code);
            var target = new Currency("CZK");//fetching table from CNB

            foreach (var currency in currencies)
            {
                if (!rates.ContainsKey(currency.Code)) continue;
                var rate = rates[currency.Code];
                yield return new ExchangeRate(currency, target, rate.ExchangeRate / rate.Amount);
            }
        }
    }
}
