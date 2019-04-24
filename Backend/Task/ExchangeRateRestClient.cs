using RestSharp;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateRestClient
    {
        private RestClient _client = new RestClient("https://www.cnb.cz/");
        private RestRequest _request = new RestRequest("cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");
        private XmlSerializer _serializer = new XmlSerializer(typeof(ResponseExchangeRate));

        public ResponseExchangeRate GetExchangeRate()
        {
            var response = _client.Execute(_request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var bytes = Encoding.UTF8.GetBytes(response.Content);
                var stream = new MemoryStream(bytes);
                var reader = new StreamReader(stream, Encoding.UTF8, true);
                return (ResponseExchangeRate)_serializer.Deserialize(reader);
            }
            return null;
        }
    }

    [XmlRoot("kurzy")]
    public partial class ResponseExchangeRate
    {
        [XmlElement("tabulka")]
        public RateTable Table { get; set; }

        [XmlAttribute("banka")]
        public string Bank { get; set; }

        [XmlAttribute("datum")]
        public string Date { get; set; }

        [XmlAttribute("poradi")]
        public byte Rank { get; set; }
    }

    public partial class RateTable
    {
        [XmlElement("radek")]
        public RateRow[] Rows { get; set; }

        [XmlAttribute("typ")]
        public string Type { get; set; }
    }

    public partial class RateRow
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }

        [XmlAttribute("mena")]
        public string Currency { get; set; }

        [XmlAttribute("mnozstvi")]
        public ushort Amount { get; set; }

        [XmlAttribute("kurz")]
        public string Rate { get; set; }

        [XmlAttribute("zeme")]
        public string Country { get; set; }
    }

}
