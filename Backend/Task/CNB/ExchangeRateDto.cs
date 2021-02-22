using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.CNB
{
    [Serializable()]
    public class ExchangeRateDto
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }

        [XmlAttribute("mnozstvi")]
        public int Quantity { get; set; }

        private static Lazy<CultureInfo> RateFormat =
            new Lazy<CultureInfo>(() =>
                new CultureInfo("cs-CZ")
                {
                    NumberFormat = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ",",
                        CurrencyDecimalSeparator = ","
                    }
                });

        [XmlIgnore]
        public decimal Rate { get; set; }

        [XmlAttribute("kurz")]
        public string RateSerialized
        {
            get => Rate.ToString(RateFormat.Value);
            set => Rate = decimal.Parse(value, RateFormat.Value);
        }
    }
}
