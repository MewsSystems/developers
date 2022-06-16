namespace ExchangeRateUpdater.Domain
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "tabulka")]
    public class BankExchangeRateLink
    {
        [XmlElement(ElementName = "radek")]
        public List<BankExchangeRateData> BankExchangeRateData { get; set; }

        [XmlAttribute(AttributeName = "typ")]
        public string Type { get; set; }
    }
}
