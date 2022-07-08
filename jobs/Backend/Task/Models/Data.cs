using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    [XmlRoot(ElementName = "tabulka")]
    public class Data
    {
        [XmlElement(ElementName = "radek")]
        public List<Entity> Entities { get; set; }
    }
}
