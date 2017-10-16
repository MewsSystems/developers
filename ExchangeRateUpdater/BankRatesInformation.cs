using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [XmlRoot("kurzy")]
    public class BankRatesInformation
    {
        [XmlAttribute("banka")]
        public string Bank { get; set; }
        [XmlAttribute("datum")]
        public string Date { get; set; }

        [XmlElement("tabulka")]
        public BankRatesTable Table { get; set; }
    }
}
