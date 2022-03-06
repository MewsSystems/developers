using System.Xml.Serialization;

namespace ExchangeRateUpdater.Providers.CNB.Parser.Xml.Elements;

[XmlRoot(ElementName = "kurzy")]
public class Rates
{
    [XmlElement(ElementName = "tabulka")]
    public Table Table { get; set; }

    [XmlAttribute(AttributeName = "banka")]
    public string Bank { get; set; }

    [XmlAttribute(AttributeName = "datum")]
    public string Date { get; set; }

    [XmlAttribute(AttributeName = "poradi")]
    public string Order { get; set; }
}