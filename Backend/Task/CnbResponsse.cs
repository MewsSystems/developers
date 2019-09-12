using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [XmlRoot("kurzy")]
    public class CnbResponsse
    {
        [XmlAttribute("banka")]
        public string Bank { get; set; }

        [XmlAttribute("datum")]
        public string Date { get; set; }

        [XmlElement("tabulka")]
        public CnbTable Table { get; set; }
    }
}
