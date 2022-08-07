namespace CurrencyExchangeService.Models
{
    using System.Xml.Serialization;

    // NOTE: Generated and updated code
    [SerializableAttribute()]
    [XmlRoot("kurzy")]
    public partial class CurrencyRateXmlResponse
    {
        private Currencies _currencies;

        [XmlElement("tabulka")]
        public Currencies Currencies
        {
            get
            {
                return this._currencies;
            }
            set
            {
                this._currencies = value;
            }
        }
    }

    public partial class Currencies
    {

        private CurrencyItem[] _currencyItem;

        [XmlElementAttribute("radek")]
        public CurrencyItem[] CurrencyItem
        {
            get
            {
                return this._currencyItem;
            }
            set
            {
                this._currencyItem = value;
            }
        }
    }

    public partial class CurrencyItem
    {
        private string _code;
        private string _name;
        private ushort _count;
        private string _rate;
        private string _country;

        [XmlAttributeAttribute("kod")]
        public string Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }

        [XmlAttributeAttribute("mena")]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [XmlAttributeAttribute("mnozstvi")]
        public ushort Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }

        [XmlAttributeAttribute("kurz")]
        public string Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
            }
        }

        [XmlAttributeAttribute("zeme")]
        public string Country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }
    }
}