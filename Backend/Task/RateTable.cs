using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [Serializable]
    [XmlRoot(@"kurzy")]
    public class RateTable
    {
        [XmlAttribute("banka")]
        public string Bank { get; set; }

        [XmlAttribute("datum")]
        public string Date { get; set; }

        [XmlAttribute("poradi")]
        public int RateOrder { get; set; }

        [XmlArray("tabulka"), XmlArrayItem("radek")]
        public List<Rate> Rates { get; set; }
    }
}