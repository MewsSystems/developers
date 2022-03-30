using System.Xml.Serialization;

namespace ExchangeRate.Infrastructure.CNB.Core.Models;

[XmlRoot(ElementName = "tabulka")]
public class Table
{
    [XmlElement(ElementName = "radek")]
    public List<Row> Rows { get; set; }

    [XmlAttribute(AttributeName = "typ")]
    public string Type { get; set; }
}
