using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    public class CnbProvider : IProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> rates = new();
            decimal value;
            HttpWebRequest request = WebRequest.Create("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml") as HttpWebRequest;
            WebResponse response = request.GetResponse();

            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new(receiveStream, Encoding.UTF8);
            var document = XmlDeserializer.XmlDeserializeFromString<CnbModel>(readStream.ReadToEnd());
            var difList = document.Tabulka.Radek.ToList().Where(a => currencies.Any(a1 => a1.Code.ToString() == a.Kod));
            
            foreach (var item in difList)
            {
                value = Convert.ToInt32(item.Mnozstvi) > 1
                    ? Math.Round(Convert.ToInt32(item.Mnozstvi) / Convert.ToDecimal(item.Kurz.Replace(',', '.')), 3)
                    : Convert.ToDecimal(item.Kurz.Replace(',', '.'));

                ExchangeRate rate = new(new Currency(item.Kod), new Currency("CZK"), value);
                rates.Add(rate);
            }
            return rates;
        }
    }
}
