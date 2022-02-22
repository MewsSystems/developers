using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.DataAccess.Models;

[XmlRoot(ElementName="radek")]
public class Radek {
    [XmlAttribute(AttributeName="kod")]
    public string Kod { get; set; }
    [XmlAttribute(AttributeName="mena")]
    public string Mena { get; set; }
    [XmlAttribute(AttributeName="mnozstvi")]
    public int Mnozstvi { get; set; }
    
    [XmlIgnore]
    public decimal Rate { get; set; }

    [XmlAttribute(AttributeName = "kurz")]
    public string RateFormatted 
    {
        get
        {
            return Rate.ToString();
        }
        set
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ",";
            Rate = decimal.Parse(value, numberFormatInfo);
        } 
    }
    [XmlAttribute(AttributeName="zeme")]
    public string Zeme { get; set; }
}