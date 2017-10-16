using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [XmlRoot("radek", IsNullable = true)]
    public class BankRate
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }
        [XmlIgnore]
        public decimal Rate { get; set; }
        [XmlAttribute("kurz")]
        public string RateValue
        {
            get { return Rate.ToString(CultureInfo.CurrentCulture); }
            set { decimal course; if (decimal.TryParse(value, out course)) Rate = course; }
        }
    }
}
