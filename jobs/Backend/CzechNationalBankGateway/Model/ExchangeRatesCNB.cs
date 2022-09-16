using System.Globalization;
using System.Xml.Serialization;

namespace CzechNationalBankAPI.Model;

[XmlRoot(ElementName = "kurzy")]
public class ExchangeRatesCNB
{
    [XmlElement(ElementName = "tabulka")]
    public TableCNB Table { get; set; }

    [XmlAttribute(AttributeName = "banka")]
    public string Bank { get; set; }

    [XmlAttribute(AttributeName = "datum")]
    public string Date { get; set; }
    public DateTime? ParsedDate
    {
        get
        {
            if (DateTime.TryParse(Date, out var date))
                return date;

            return null;
        }
    }

    [XmlAttribute(AttributeName = "poradi")]
    public int Order { get; set; }
}