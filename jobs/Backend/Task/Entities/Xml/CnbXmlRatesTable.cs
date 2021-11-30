using System;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Entities.Xml
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public class CnbXmlRatesTable

    {

        private CnbXmlRate[] _ratesField;


        /// <remarks/>
        [XmlElement("radek", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CnbXmlRate[] Rates
        {
            get { return _ratesField; }
            set { _ratesField = value; }
        }


    }
}