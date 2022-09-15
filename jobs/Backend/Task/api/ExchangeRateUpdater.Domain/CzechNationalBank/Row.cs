using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Domain.CzechNationalBank;

[XmlRoot(ElementName="radek")]
public class Row {
  
  [XmlAttribute(AttributeName="kod")]
  public string Code { get; set; }
  
  [XmlAttribute(AttributeName="mena")]
  public string Currency { get; set; }
  
  [XmlAttribute(AttributeName="mnozstvi")]
  public int Amount { get; set; }
  
  [XmlAttribute(AttributeName="kurz")]
  public string RateDependingOnAmount { get; set; }
  public decimal RateDependingOnAmountDec
  {
    get
    {
      try
      {
        return Convert.ToDecimal(this.RateDependingOnAmount, new CultureInfo("cs-CZ"));
      }
      catch (Exception ex)
      {
        return 0;
      }
    }
  }
  
  public decimal Rate => Math.Round(RateDependingOnAmountDec / Amount, 3);
  
  [XmlAttribute(AttributeName="zeme")]
  public string Country { get; set; }
}