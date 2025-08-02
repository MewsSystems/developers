using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models.Countries.CZE;

public class CzeExchangeRateTable
{
    [XmlAttribute("typ")]
    public string Type { get; set; }

    [XmlElement("radek")]
    public List<CzeExchangeRate> Rates { get; set; }
}