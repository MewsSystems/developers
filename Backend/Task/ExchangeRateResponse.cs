using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [Serializable()]
    [XmlRoot("kurzy")]
    public class ExchangeRateResponse
    {
        [XmlElement("tabulka")]
        public InnerTable Table { get; set; }

        [XmlAttribute("poradi")]
        public byte Order { get; set; }

        [XmlAttribute("banka")]
        public string Bank { get; set; }

        [XmlAttribute("datum")]
        public string Date { get; set; } 
    }

    [Serializable()]
    public class InnerTable
    {
        [XmlElement("radek")]
        public TableRow[] RatesRows { get; set; }

        [XmlAttribute("typ")]
        public string Type { get; set; }
    }

    [Serializable()]
    public class TableRow
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }

        [XmlAttribute("mena")]
        public string Currency { get; set; }

        [XmlAttribute("mnozstvi")]
        public ushort Amount { get; set; }

        [XmlAttribute("kurz")]
        public string Rate { get; set; }

        [XmlAttribute("zeme")]
        public string Country { get; set; }
    }
}
