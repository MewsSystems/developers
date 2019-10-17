using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ExchangeRateUpdater
{
    public interface IRateDataProvider
    {
        Dictionary<string, CurrencyRate> Data { get; }
        CurrencyRate ProvidedFor { get; }
    }

    /// <summary>
    /// This Rate provider provides conversion rates from CZK to other currencies provided by Czech National Bank
    /// </summary>
    public class CNBRateDataProvider : IRateDataProvider
    {
        private static readonly string[] sourceUrls = { "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml", "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.xml" };

        private DateTime lastUpdate;

        private Dictionary<string, CurrencyRate> cnbDataCache;

        private DateTime Today1430 => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 14, minute: 30, 0);
        public CurrencyRate ProvidedFor => new CurrencyRate { Currency = new Currency("CZK"), Amount = 1, Rate = 1};

        public Dictionary<string, CurrencyRate> Data
        {
            get
            {
                if (cnbDataCache == null || !cnbDataCache.Any() || lastUpdate < Today1430)
                {
                    lastUpdate = DateTime.Now;
                    cnbDataCache = DownloadData(sourceUrls).ToDictionary(x => x.Currency.Code);
                }

                return cnbDataCache;
            }
        }

        private IEnumerable<CurrencyRate> DownloadData(string[] sources)
        {
            foreach (var source in sources)
            {
                using (var reader = new XmlTextReader(source))
                {
                    while (reader.Read())
                    {
                        if ((reader.NodeType != XmlNodeType.Element) && (reader.Name != "radek")) continue;
                        if (!reader.HasAttributes) continue;

                        var code = reader.GetAttribute("kod");
                        
                        if (!decimal.TryParse(reader.GetAttribute("kurz"), out decimal rate)) continue;
                        if (!decimal.TryParse(reader.GetAttribute("mnozstvi"), out decimal amount)) continue;

                        yield return new CurrencyRate
                        {
                            Currency = new Currency(code),
                            Amount = amount,
                            Rate = rate
                        };
                    }
                }
            }
        }
    }

    public class CurrencyRate
    {
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
    }
}
