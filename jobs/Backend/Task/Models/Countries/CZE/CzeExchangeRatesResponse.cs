using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models.Countries.CZE;

[XmlRoot("kurzy")]
public class CzeExchangeRatesResponse
{
    [XmlAttribute("banka")]
    public string Bank { get; set; }

    [XmlAttribute("datum")]
    public string Date { get; set; }

    [XmlAttribute("poradi")]
    public string Sequence { get; set; }

    [XmlElement("tabulka")]
    public CzeExchangeRateTable Table { get; set; }
}
