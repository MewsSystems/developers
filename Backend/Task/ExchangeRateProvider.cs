using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string exchangeRateUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // download xml document by url
            var webClient = new WebClient();
            var currenciesData = webClient.DownloadString(exchangeRateUrl);
            if(currenciesData == null || !currenciesData.Any())
                return Enumerable.Empty<ExchangeRate>();

            var xmlSerialiser = new XmlSerializer(typeof(CnbResponsse));
            using (var reader = new StringReader(currenciesData))
            {
                var cnbResponse = (CnbResponsse)xmlSerialiser.Deserialize(reader);

                var currenciesCodes = cnbResponse.Table?.rows?.Where(x => currencies.Any(c => c.Code == x?.Code));
                if(currenciesData == null || !currenciesData.Any())
                    return Enumerable.Empty<ExchangeRate>();

                return currenciesCodes.SelectMany(er => currenciesCodes, (source, target) =>
                {
                    return new ExchangeRate(new Currency(source.Code), new Currency(target.Code),
                                            decimal.Parse(source.Rate) * target.Amount / (decimal.Parse(target.Rate) * source.Amount));
                }).Where(x => x.SourceCurrency.Code != x.TargetCurrency.Code);
            }
        }
    }
}
