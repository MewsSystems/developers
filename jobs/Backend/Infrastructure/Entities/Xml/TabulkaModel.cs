using System.Collections.Generic;
using System.Xml.Serialization;

namespace Infrastructure.Entities.Xml
{
    public class TabulkaModel
    {
        [XmlAttribute("typ")]
        public string Description { get; set; }

        [XmlElement(ElementName = "radek")]
        public List<RadekModel> Rates { get; set; }
    }
}