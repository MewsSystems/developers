using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public class CnbRow
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }

        [XmlAttribute("mena")]
        public string Name { get; set; }

        [XmlAttribute("mnozstvi")]
        public int Amount { get; set; }

        [XmlAttribute("kurz")]
        public string Rate { get; set; }

        [XmlAttribute("zeme")]
        public string Country { get; set; }
    }
}
