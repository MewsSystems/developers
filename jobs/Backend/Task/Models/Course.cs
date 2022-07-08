using System;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    [XmlRoot(ElementName = "kurzy")]
    public class Course
    {
        [XmlElement(ElementName = "tabulka")]
        public Data Data { get; set; }

        [XmlAttribute(AttributeName = "banka")]
        public string Bank { get; set; }
    }
}