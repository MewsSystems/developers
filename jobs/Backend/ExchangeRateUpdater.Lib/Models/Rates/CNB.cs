using ExchangeRateUpdater.Exceptions;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models.Rates;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
public partial class kurzy
{

    private kurzyTabulka tabulkaField;

    private string bankaField;

    private string datumField;

    private byte poradiField;

    /// <remarks/>
    public kurzyTabulka tabulka
    {
        get
        {
            return tabulkaField;
        }
        set
        {
            tabulkaField = value;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string banka
    {
        get
        {
            return bankaField;
        }
        set
        {
            bankaField = value;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string datum
    {
        get
        {
            return datumField;
        }
        set
        {
            datumField = value;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public byte poradi
    {
        get
        {
            return poradiField;
        }
        set
        {
            poradiField = value;
        }
    }
}

/// <remarks/>
[Serializable()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
public partial class kurzyTabulka
{

    private kurzyTabulkaRadek[] radekField;

    private string typField;

    /// <remarks/>
    [XmlElement("radek")]
    public kurzyTabulkaRadek[] radek
    {
        get
        {
            return radekField;
        }
        set
        {
            radekField = value;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string typ
    {
        get
        {
            return typField;
        }
        set
        {
            typField = value;
        }
    }
}

/// <remarks/>
[Serializable()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
public partial class kurzyTabulkaRadek
{

    private string kodField;

    private string menaField;

    private ushort mnozstviField;

    private string kurzField;

    private string zemeField;

    /// <remarks/>
    [XmlAttribute()]
    public string kod
    {
        get
        {
            return kodField;
        }
        set
        {
            kodField = value;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string mena
    {
        get
        {
            return menaField;
        }
        set
        {
            menaField = value;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public ushort mnozstvi
    {
        get
        {
            return mnozstviField;
        }
        set
        {
            mnozstviField = value;
        }
    }

    ///// <remarks/>
    //[System.Xml.Serialization.XmlAttributeAttribute()]
    //public string kurz
    //{
    //    get
    //    {
    //        return this.kurzField;
    //    }
    //    set
    //    {
    //        this.kurzField = value;
    //    }
    //}

    [XmlIgnore]
    public decimal kurzUseable { get; set; }

    [XmlAttribute("kurz")]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public string kurz
    {
        get
        {
            return kurzField;
        }
        set
        {
            kurzField = value;
            if (!decimal.TryParse(kurzField, NumberStyles.Any, CultureInfo.CreateSpecificCulture("cs-CZ"), out var kurz))
                throw new RateParseException($"unable to parse exchange rate value '{kurzField}'");
            else
                kurzUseable = kurz;
        }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string zeme
    {
        get
        {
            return zemeField;
        }
        set
        {
            zemeField = value;
        }
    }
}

