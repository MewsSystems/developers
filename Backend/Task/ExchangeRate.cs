using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateList
    {
        [System.Xml.Serialization.XmlElement("radek")]
        public List<ExchangeRate> one = new List<ExchangeRate>();
    }
    
    public class ExchangeRate
    {
        

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }
        public ExchangeRate()
        {

        }
        [System.Xml.Serialization.XmlAttribute("kod")]
        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        //[System.Xml.Serialization.XmlElement("kurz")]
        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
