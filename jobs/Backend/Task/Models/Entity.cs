using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    [XmlRoot(ElementName = "radek")]
    public class Entity
    {
        [XmlAttribute(AttributeName = "kod")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "kurz")]
        public string ExchangeRate { get; set; }
    }
}