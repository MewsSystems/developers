using System.Xml.Serialization;

namespace RomanianNationalBank.Models;

/// <summary>
/// Represents the root element of the BNR (Romanian National Bank) XML response.
/// The BNR provides exchange rates via XML at: https://www.bnr.ro/nbrfxrates.xml
/// XML Structure: DataSet/Body/Cube/Rate with namespace http://www.bnr.ro/xsd
/// </summary>
[XmlRoot("DataSet")]
public class BnrDataSet
{
    /// <summary>
    /// Header element containing metadata about the response.
    /// </summary>
    [XmlElement("Header")]
    public BnrHeader? Header { get; set; }

    /// <summary>
    /// Body element containing the exchange rate data structure.
    /// </summary>
    [XmlElement("Body")]
    public BnrBody? Body { get; set; }
}

/// <summary>
/// Represents the Header element containing metadata.
/// </summary>
public class BnrHeader
{
    /// <summary>
    /// Publisher of the data (e.g., "National Bank of Romania").
    /// </summary>
    [XmlElement("Publisher")]
    public string? Publisher { get; set; }

    /// <summary>
    /// Publishing date of the data.
    /// </summary>
    [XmlElement("PublishingDate")]
    public string? PublishingDate { get; set; }

    /// <summary>
    /// Message type (e.g., "DR" for Daily Rates).
    /// </summary>
    [XmlElement("MessageType")]
    public string? MessageType { get; set; }
}

/// <summary>
/// Represents the Body element containing exchange rate data.
/// </summary>
public class BnrBody
{
    /// <summary>
    /// Exchange rates for a specific date.
    /// The BNR typically publishes one Cube per day.
    /// </summary>
    [XmlElement("Cube")]
    public List<BnrCube> Cubes { get; set; } = new();
}

/// <summary>
/// Represents a collection of exchange rates for a specific date.
/// </summary>
public class BnrCube
{
    /// <summary>
    /// The date for which these rates are valid (format: yyyy-MM-dd).
    /// </summary>
    [XmlAttribute("date")]
    public string? Date { get; set; }

    /// <summary>
    /// List of exchange rates for different currencies.
    /// All rates are relative to RON (Romanian Leu) as the base currency.
    /// </summary>
    [XmlElement("Rate")]
    public List<BnrRate> Rates { get; set; } = new();
}

/// <summary>
/// Represents an individual exchange rate.
/// Format: 1 [currency] = [value] RON * multiplier
/// Example: 1 USD = 4.1234 RON
/// </summary>
public class BnrRate
{
    /// <summary>
    /// ISO 4217 currency code (e.g., USD, EUR, GBP).
    /// </summary>
    [XmlAttribute("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// Multiplier for the exchange rate.
    /// Usually 1, but some currencies may use different multipliers.
    /// </summary>
    [XmlAttribute("multiplier")]
    public int Multiplier { get; set; } = 1;

    /// <summary>
    /// The exchange rate value.
    /// Represents how many RON equal one unit of the foreign currency (adjusted by multiplier).
    /// </summary>
    [XmlText]
    public decimal Value { get; set; }
}
