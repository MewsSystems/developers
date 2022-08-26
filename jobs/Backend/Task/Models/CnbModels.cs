using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
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
            return this.tabulkaField;
        }
        set
        {
            this.tabulkaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string banka
    {
        get
        {
            return this.bankaField;
        }
        set
        {
            this.bankaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string datum
    {
        get
        {
            return this.datumField;
        }
        set
        {
            this.datumField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte poradi
    {
        get
        {
            return this.poradiField;
        }
        set
        {
            this.poradiField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class kurzyTabulka
{

    private kurzyTabulkaRadek[] radekField;

    private string typField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("radek")]
    public kurzyTabulkaRadek[] radek
    {
        get
        {
            return this.radekField;
        }
        set
        {
            this.radekField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string typ
    {
        get
        {
            return this.typField;
        }
        set
        {
            this.typField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class kurzyTabulkaRadek
{

    private string kodField;

    private string menaField;

    private ushort mnozstviField;

    private string kurzField;

    private string zemeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string kod
    {
        get
        {
            return this.kodField;
        }
        set
        {
            this.kodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string mena
    {
        get
        {
            return this.menaField;
        }
        set
        {
            this.menaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort mnozstvi
    {
        get
        {
            return this.mnozstviField;
        }
        set
        {
            this.mnozstviField = value;
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
            return this.kurzField;
            //return kurz.ToString(CultureInfo.CreateSpecificCulture("cs-CZ"));
        }
        set
        {
            this.kurzField = value;
            //TODO: store base culture in appsettings
            if (!decimal.TryParse(this.kurzField, NumberStyles.Any, CultureInfo.CreateSpecificCulture("cs-CZ"), out var kurz))
                throw new Exception("unable to parse exchange rate value!");
            else
                this.kurzUseable = kurz;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string zeme
    {
        get
        {
            return this.zemeField;
        }
        set
        {
            this.zemeField = value;
        }
    }
}

