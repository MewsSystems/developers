using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class CnbResponseTableRow
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