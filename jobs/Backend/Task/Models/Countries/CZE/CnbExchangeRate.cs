using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models.Countries.CZE;

public class CnbExchangeRate
{
    [XmlAttribute("kod")]
    public string Code { get; set; }

    [XmlAttribute("mena")]
    public string CurrencyName { get; set; }

    [XmlAttribute("mnozstvi")]
    public int Amount { get; set; }

    [XmlAttribute("kurz")]
    public string RateRaw { get; set; }  // Ej: "13,854"

    [XmlAttribute("zeme")]
    public string Country { get; set; }

    [XmlIgnore]
    public decimal Rate => decimal.Parse(RateRaw.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
}
