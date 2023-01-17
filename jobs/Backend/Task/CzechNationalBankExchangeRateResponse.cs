using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [XmlRoot("kurzy")]
    public class CzechNationalBankExchangeRateResponse
    {
        [XmlAttribute("banka")]
        public string BankCode { get; set; }
        
        [XmlAttribute("datum")]
        public string Date { get; set; }
        
        [XmlElement("tabulka")]
        public CnbExchangeRateList CzechNationalBankExchangeRateList { get; set; }
    }
    
    [XmlRoot("tabulka")]
    public class CnbExchangeRateList
    {
        [XmlElement("radek")]
        public List<CnbExchangeRate> Rates { get; set; }
    }
    
    [XmlRoot("radek")]
    public class CnbExchangeRate
    {
        [XmlAttribute("zeme")]
        public string Country { get; set; }

        [XmlAttribute("mena")]
        public string Currency { get; set; }

        [XmlAttribute("mnozstvi")]
        public string Amount { get; set; }

        [XmlAttribute("kod")]
        public string Code { get; set; }

        [XmlAttribute("kurz")]
        public string Rate { get; set; }
    }
}
