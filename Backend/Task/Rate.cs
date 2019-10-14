using System;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    [Serializable]
    public class Rate
    {
        [XmlAttribute("kod")]
        public string Code { get; set; }
        [XmlAttribute("mena")]
        public string CurrencyName { get; set; }
        [XmlAttribute("mnozstvi")]
        public int Amount { get; set; }
        [XmlIgnore]
        public decimal ExchangeRate { get; set; }
        [XmlAttribute("poradi")]
        public string CountryName { get; set; }

        /// <summary>
        /// Used for deserialization. Be ware of culture info, when it's used for serialize.
        /// </summary>
        [XmlAttribute("kurz")]
        public string ExchangeRateSerialize
        {
            get => $"{ExchangeRate}";
            set => ExchangeRate = decimal.Parse(value.Replace(',', '.'));
        }

    }
}