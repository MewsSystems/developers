using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Data.Models
{
    [Serializable()]
    [XmlRoot("kurzy")]
    public class CNBExchangeRate
    {
        [XmlElement(ElementName = "tabulka")]
        public RatesTable RatesTable { get; set; }
    }

    [XmlRoot(ElementName = "kurzy")]
    public class RatesTable
    {
        [XmlElement(ElementName = "radek")]
        public List<Rate> ExchangeRates { get; set; }
    }

    public class Rate
    {
        [XmlAttribute(AttributeName = "kod")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "mnozstvi")]
        public int Amount { get; set; }

        [XmlAttribute(AttributeName = "kurz")]
        public string ExchangeRate { get; set; }
    }

}
