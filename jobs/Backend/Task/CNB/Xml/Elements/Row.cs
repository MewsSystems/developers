using System.Xml.Serialization;

namespace ExchangeRateUpdater.CNB.Xml.Elements;

[XmlRoot(ElementName = "radek")]
public class Row
{
    [XmlAttribute(AttributeName = "kod")]
    public string Code { get; set; }
    [XmlAttribute(AttributeName = "mena")]
    public string Currency { get; set; }
    [XmlAttribute(AttributeName = "mnozstvi")]
    public int Amount { get; set; }
    [XmlAttribute(AttributeName = "kurz")]
    public string Rate { get; set; }
    [XmlAttribute(AttributeName = "zeme")]
    public string Country { get; set; }
}