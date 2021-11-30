using System;
using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Entities.Xml
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public class CnbXmlRate
    {

        private string _currencyCodeField;

        private  int _multiplierField;

        private decimal _valueField;

        private static NumberFormatInfo numberFormatInfo = new  NumberFormatInfo (){ NumberDecimalSeparator = "," };
        /// <remarks/>
        [XmlAttribute("kod")]
        public string CurrencyCode
        {
            get { return _currencyCodeField; }
            set { _currencyCodeField = value; }
        }

        /// <remarks/>
        [XmlAttribute("mnozstvi")]
        public int Multiplier
        {
            get { return _multiplierField; }
            set { _multiplierField = value; }
        }

        /// <remarks/>
        [XmlAttribute("kurz")]
        public string ValueString
        {
            get { return _valueField.ToString(); }
            set { _valueField = Decimal.Parse(value, numberFormatInfo ); }
        }

        [XmlIgnore]
        public decimal Value => _valueField;
    }
}
