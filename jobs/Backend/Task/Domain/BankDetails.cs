namespace ExchangeRateUpdater.Domain
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "kurzy")]
    public class BankDetails
    {
        [XmlAttribute("banka")]
        public string Bank { get; set; }

        public DateTime Date { get; set; }

        [XmlAttribute("datum")]
        public string DateXml
        {
            get => Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            set => Date = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        [XmlAttribute("poradi")]
        public int Advice { get; set; }

        [XmlElement(ElementName = "tabulka")]
        public BankExchangeRateLink BankExchangeRateLink { get; set; }
    }
}
