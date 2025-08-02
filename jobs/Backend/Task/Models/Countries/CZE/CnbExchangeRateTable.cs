using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models.Countries.CZE;

public class CnbExchangeRateTable
{
    [XmlAttribute("typ")]
    public string Type { get; set; }

    [XmlElement("radek")]
    public List<CnbExchangeRate> Rates { get; set; }
}