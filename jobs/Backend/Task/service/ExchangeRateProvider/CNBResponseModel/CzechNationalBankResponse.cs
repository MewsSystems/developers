using System.Xml.Serialization;

namespace ExchangeRateProviderCzechNationalBank.CNBResponseModel
{
    [XmlRoot(ElementName = "radek")]
    public class Radek
    {
        [XmlAttribute(AttributeName = "kod")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "mena")]
        public string Currency { get; set; }

        [XmlAttribute(AttributeName = "mnozstvi")]
        public string Amount { get; set; }

        [XmlAttribute(AttributeName = "kurz")]
        public string Rate { get; set; }

        [XmlAttribute(AttributeName = "zeme")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "tabulka")]
    public class Tabulka
    {
        [XmlElement(ElementName = "radek")]
        public List<Radek> Line { get; set; }

        [XmlAttribute(AttributeName = "typ")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "kurzy")]
    public class Kurzy
    {
        [XmlElement(ElementName = "tabulka")]
        public Tabulka Table { get; set; }

        [XmlAttribute(AttributeName = "banka")]
        public string Bank { get; set; }

        [XmlAttribute(AttributeName = "datum")]
        public string Date { get; set; }

        [XmlAttribute(AttributeName = "poradi")]
        public string Order { get; set; }
    }
}
