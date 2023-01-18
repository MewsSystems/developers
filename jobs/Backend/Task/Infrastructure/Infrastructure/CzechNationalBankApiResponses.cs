using System.Xml.Serialization;

namespace Infrastructure
{
    [XmlRoot("kurzy")]
    public class CzechNationalBankExchangeRateResponse
    {
        [XmlAttribute("banka")]
        public string BankCode { get; set; } = null!;

        [XmlAttribute("datum")]
        public string Date { get; set; } = null!;

        [XmlElement("tabulka")]
        public CnbExchangeRateList CzechNationalBankExchangeRateList { get; set; } = null!;
    }
    
    [XmlRoot("tabulka")]
    public class CnbExchangeRateList
    {
        [XmlElement("radek")]
        public List<CnbExchangeRate> Rates { get; set; } = null!;
    }
    
    [XmlRoot("radek")]
    public class CnbExchangeRate
    {
        [XmlAttribute("zeme")]
        public string Country { get; set; } = null!;

        [XmlAttribute("mena")]
        public string Currency { get; set; } = null!;

        [XmlAttribute("mnozstvi")]
        public string Amount { get; set; } = null!;

        [XmlAttribute("kod")]
        public string Code { get; set; } = null!;

        [XmlAttribute("kurz")]
        public string Rate { get; set; } = null!;
    }
}
