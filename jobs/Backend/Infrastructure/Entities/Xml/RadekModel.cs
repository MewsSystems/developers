using System.Xml.Serialization;

namespace Infrastructure.Entities.Xml
{
    public class RadekModel
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }

        [XmlAttribute("mena")]
        public string Coin { get; set; }

        [XmlAttribute("mnozstvi")]
        public string Amount { get; set; }

        [XmlAttribute("kurz")]
        public string CurrentRate { get; set; }

        [XmlAttribute("zeme")]
        public string Country { get; set; }
    }
}