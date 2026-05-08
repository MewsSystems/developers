using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Domain.ApiClients.Models;

[XmlRoot("kurzy")]
public class ExchangeRatesResponse
{
    [XmlElement("tabulka")]
    public ExchangeRateTable[] Tables { get; set; } = [];
}

public class ExchangeRateTable
{
    [XmlElement("radek")]
    public ExchangeRateRow[] Rows { get; set; } = [];
}

public class ExchangeRateRow
{
    [XmlAttribute("kod")]
    public string CurrencyCode { get; set; } = string.Empty;

    [XmlAttribute("mena")]
    public string CurrencyName { get; set; } = string.Empty;

    [XmlAttribute("kurz")]
    public string RateString { get; set; } = string.Empty;

    [XmlAttribute("mnozstvi")]
    public string AmountString { get; set; } = "1";

    [XmlIgnore]
    public decimal Rate => decimal.Parse(RateString, NumberStyles.Number, new CultureInfo("cs-CZ"));

    [XmlIgnore]
    public decimal Amount => decimal.Parse(AmountString, NumberStyles.Number, new CultureInfo("cs-CZ"));
}