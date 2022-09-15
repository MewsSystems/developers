using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Domain.CzechNationalBank;

[XmlRoot(ElementName="kurzy")]
public class ExchangeRateHeader : ExchangeRateHeaderBase {
  
  [XmlElement(ElementName="tabulka")]
  public Table Table { get; set; }
  
  [XmlAttribute(AttributeName="banka")]
  public string Bank { get; set; }
  
  [XmlAttribute(AttributeName="datum")]
  public string Date { get; set; }
  public DateTime DateDT
  {
    get
    {
      try
      {
        return DateTime.Parse(this.Date, new CultureInfo("cs-CZ"));
      }
      catch (Exception ex)
      {
        return DateTime.MinValue;
      }
    }
  }
  public string DateAsInvariantCultureDateString => DateDT.Date.ToString(CultureInfo.InvariantCulture);

  [XmlAttribute(AttributeName="poradi")]
  public int Order { get; set; }
}