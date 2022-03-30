using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRate.Infrastructure.CNB.Core.Models;

[XmlRoot(ElementName = "kurzy")]
public class ExchangeRate
{
    [XmlElement(ElementName = "tabulka")]
    public Table Table { get; set; }

    [XmlAttribute(AttributeName = "banka")]
    public string Bank { get; set; }

    [XmlIgnore]
    public DateTime Date { get; set; }

    [XmlAttribute("datum")]
    public string DateFormatted
    {
        get => Date.ToString(CNBConstants.DateFormat, CultureInfo.InvariantCulture);
        set => Date = DateTime.ParseExact(value, CNBConstants.DateFormat, CultureInfo.InvariantCulture);
    }

    [XmlAttribute(AttributeName = "poradi")]
    public int OrderNo { get; set; }
}
