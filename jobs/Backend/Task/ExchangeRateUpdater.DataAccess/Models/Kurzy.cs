using System.Xml.Serialization;

namespace ExchangeRateUpdater.DataAccess.Models;

[XmlRoot(ElementName="kurzy")]
public class Kurzy {
    [XmlElement(ElementName="script")]
    public string Script { get; set; }
    [XmlElement(ElementName="tabulka")]
    public Tabulka Tabulka { get; set; }
    [XmlAttribute(AttributeName="banka")]
    public string Banka { get; set; }
    [XmlAttribute(AttributeName="datum")]
    public string Datum { get; set; }
    [XmlAttribute(AttributeName="poradi")]
    public string Poradi { get; set; }
}







