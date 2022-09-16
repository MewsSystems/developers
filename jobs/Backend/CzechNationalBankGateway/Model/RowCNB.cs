using System.Globalization;
using System.Xml.Serialization;


namespace CzechNationalBankAPI.Model
{
    [XmlRoot(ElementName = "radek")]
    public class RowCNB
    {

        [XmlAttribute(AttributeName = "kod")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "mena")]
        public string Currency { get; set; }

        [XmlAttribute(AttributeName = "mnozstvi")]
        public int Amount { get; set; }

        [XmlAttribute(AttributeName = "kurz")]
        public string RateString { get; set; }
        public decimal? ParsedRate
        {
            get
            {
                if (decimal.TryParse(RateString, out var amount))
                    return amount;

                return null;
            }
        }

        public decimal? Rate => ParsedRate != null ? Math.Round(ParsedRate.Value / Amount, 3) : null;

        [XmlAttribute(AttributeName = "zeme")]
        public string Country { get; set; }
    }

}
