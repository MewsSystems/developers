using System.Xml.Serialization;

namespace ExchangeRateUpdater.Domain.CzechNationalBank;

[XmlRoot(ElementName="tabulka")]
public class Table {
  
  [XmlElement(ElementName="radek")]
  public List<Row> Row { get; set; }
  
  [XmlAttribute(AttributeName="typ")]
  public string Typ { get; set; }
}