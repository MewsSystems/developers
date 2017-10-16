using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [XmlRoot("tabulka", IsNullable = true)]
    public class BankRatesTable
    {
        [XmlAttribute("typ")]
        public string Type { get; set; }
        [XmlElement("radek")]
        public BankRate[] Rates { get; set; }
    }


}
