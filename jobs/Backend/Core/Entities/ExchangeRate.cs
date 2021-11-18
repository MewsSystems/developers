using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Entities
{

    [XmlRoot(ElementName = "radek")]
    public class Row
    {

        [XmlAttribute(AttributeName = "kod")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "mena")]
        public string Currency { get; set; }

        [XmlAttribute(AttributeName = "mnozstvi")]
        public int Quantity { get; set; }


        [XmlAttribute(AttributeName = "kurz")]
        public string ExchangeRate { get; set; }

        [XmlAttribute(AttributeName = "zeme")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "tabulka")]
    public class Table
    {

        [XmlElement(ElementName = "radek")]
        public List<Row> Rows { get; set; }

        [XmlAttribute(AttributeName = "typ")]
        public string TableType { get; set; }
    }

    [XmlRoot(ElementName = "kurzy")]
    public class ExchangeRate
    {

        [XmlElement(ElementName = "tabulka")]
        public Table Table { get; set; }

        [XmlAttribute(AttributeName = "banka")]
        public string Bank { get; set; }

        [XmlAttribute(AttributeName = "datum")]
        public string Date { get; set; }

        [XmlAttribute(AttributeName = "poradi")]
        public int Rank { get; set; }
    }
}

