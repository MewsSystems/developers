namespace ExchangeRateUpdater.Domain
{
    using System.Xml.Serialization;
    using ExchangeRateUpdater.DI;

    [XmlRoot(ElementName = "radek")]
    public class BankExchangeRateData
    {
        [XmlAttribute(AttributeName = "kod")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "mena")]
        public string CurrencyName { get; set; }

        [XmlAttribute(AttributeName = "mnozstvi")]
        public int Amount { get; set; }

        [XmlIgnore]
        public decimal Rate { get; set; }

        [XmlAttribute(AttributeName = "kurz")]
        public string RateFormatted
        {
            get => Rate.ToString(Configuration.NumberSeprator);
            set => Rate = decimal.Parse(value, Configuration.NumberSeprator);
        }

        public decimal CalculatedRate
        {
            get => Rate / Amount;
        }

        [XmlAttribute(AttributeName = "zeme")]
        public string Country { get; set; }
    }
}
