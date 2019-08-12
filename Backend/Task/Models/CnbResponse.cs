using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    [XmlRoot("kurzy")]
    public class CnbResponse
    {
        [XmlElement("tabulka")]
        public CnbResponseTable Table { get; set; }

        [XmlAttribute("banka")]
        public string Bank { get; set; }

        [XmlAttribute("datum")]
        public string Date { get; set; }

        [XmlAttribute("poradi")]
        public byte Rank { get; set; }
    }
}