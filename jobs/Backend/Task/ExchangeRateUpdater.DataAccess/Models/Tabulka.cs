using System.Xml.Serialization;

namespace ExchangeRateUpdater.DataAccess.Models;

[XmlRoot(ElementName="tabulka")]
public class Tabulka {
    [XmlElement(ElementName="radek")]
    public List<Radek> Radek { get; set; }
    [XmlAttribute(AttributeName="typ")]
    public string Typ { get; set; }
}