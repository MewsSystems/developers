using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Infrastructure.Entities.Xml
{
    [Serializable, XmlRoot(ElementName = "kurzy")]
    public class KurzyModel : IGenericEntity
    {
        [XmlAttribute("banka")]
        public string Bank { get; set; }

        [XmlAttribute("datum")]
        public string Date { get; set; }

        [XmlAttribute("poradi")]
        public string Serial { get; set; }

        [XmlElement(ElementName = "tabulka")]
        public TabulkaModel Table { get; set; }

        public IEnumerable<GenericRate> ToGenericEntity()
        {
            return Table.Rates
                .Select(item =>
                    new GenericRate(item))
                .ToList();
        }
    }
}
