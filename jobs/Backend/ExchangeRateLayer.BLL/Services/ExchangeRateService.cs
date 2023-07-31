using ExchangeRateLayer.BLL.Objects;
using ExchangeRateLayer.BLL.Services.Abstract;
using System.Net;
using System.Xml;

namespace ExchangeRateLayer.BLL.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private string defaultMnozstvi = "1";

        public List<ExchangeRate> GetAllExchangeRates()
        {
            var response = SendRequest("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return ReadDataFromResponse(response);
        }

        private List<ExchangeRate> ReadDataFromResponse(HttpWebResponse response)
        {
            List<ExchangeRate> ExchangeRates = new List<ExchangeRate>();
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var data = stream.ReadToEnd();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(data);

                var tabulka = xmlDocument.SelectSingleNode("//tabulka");
                ExchangeRates.Capacity = tabulka.ChildNodes.Count;

                foreach (XmlNode childnode in tabulka.ChildNodes)
                {
                    var kod = childnode.Attributes.GetNamedItem("kod").Value;
                    var mnozstvi = childnode.Attributes.GetNamedItem("mnozstvi").Value;
                    decimal kurz;
                    decimal.TryParse(childnode.Attributes.GetNamedItem("kurz").Value, out kurz);

                    if (mnozstvi.Equals(defaultMnozstvi))
                        ExchangeRates.Add(new ExchangeRate(kurz, new Currency(kod)));
                    else
                    {
                        decimal valueMnozstvi;
                        decimal.TryParse(mnozstvi, out valueMnozstvi);
                        kurz /= valueMnozstvi;
                        ExchangeRates.Add(new ExchangeRate(kurz, new Currency(kod)));
                    }
                }
            }
            return ExchangeRates;
        }

        private HttpWebResponse SendRequest(string conection)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(conection);
            var response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetSelectedExchangeRates(IEnumerable<Currency> currencies)
        {
            var ExchangeRates = GetAllExchangeRates();
            List<ExchangeRate> output = new List<ExchangeRate>();

            if (ExchangeRates == null)
                return output;

            foreach (var item in currencies)
            {
                var currency = ExchangeRates.FirstOrDefault(i => i.SourceCurrency.Code.Equals(item.Code));
                if (currency != null)
                    output.Add(currency);
            }
            return output;
        }
    }
}
