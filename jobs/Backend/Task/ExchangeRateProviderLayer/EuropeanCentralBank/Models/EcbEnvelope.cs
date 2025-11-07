using System.Xml.Serialization;

namespace EuropeanCentralBank.Models;

/// <summary>
/// Represents the root element of the ECB (European Central Bank) XML response.
/// The ECB provides exchange rates via XML at: https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml
/// Format: Triple-nested Cube structure (Envelope > Cube > Cube > Cube)
/// </summary>
[XmlRoot("Envelope")]
public class EcbEnvelope
{
    /// <summary>
    /// Subject of the data (typically "Reference rates").
    /// </summary>
    [XmlElement("subject")]
    public string? Subject { get; set; }

    /// <summary>
    /// Sender information (European Central Bank).
    /// </summary>
    [XmlElement("Sender")]
    public EcbSender? Sender { get; set; }

    /// <summary>
    /// Outer Cube element containing date cubes.
    /// This is the outermost container in the triple-nested structure.
    /// </summary>
    [XmlElement("Cube")]
    public EcbOuterCube? Cube { get; set; }
}

/// <summary>
/// Represents the Sender element containing sender information.
/// </summary>
public class EcbSender
{
    /// <summary>
    /// Name of the sender organization (e.g., "European Central Bank").
    /// </summary>
    [XmlElement("name")]
    public string? Name { get; set; }
}

/// <summary>
/// Represents the outermost Cube element (container for date cubes).
/// ECB uses triple-nested Cube structure: Cube > Cube > Cube
/// </summary>
public class EcbOuterCube
{
    /// <summary>
    /// Collection of date-specific Cube elements.
    /// Daily file typically has one, historical files may have multiple.
    /// </summary>
    [XmlElement("Cube")]
    public List<EcbDateCube> DateCubes { get; set; } = new();
}

/// <summary>
/// Represents a date-specific Cube element containing exchange rates for a specific date.
/// This is the middle level in the triple-nested structure.
/// </summary>
public class EcbDateCube
{
    /// <summary>
    /// The date for which these rates are valid (format: YYYY-MM-DD).
    /// </summary>
    [XmlAttribute("time")]
    public string? Time { get; set; }

    /// <summary>
    /// Collection of rate Cube elements.
    /// Each represents an exchange rate for a specific currency.
    /// </summary>
    [XmlElement("Cube")]
    public List<EcbRate> Rates { get; set; } = new();
}

/// <summary>
/// Represents an individual exchange rate Cube element.
/// This is the innermost level in the triple-nested structure.
/// Format: 1 EUR = [rate] [currency]
/// </summary>
public class EcbRate
{
    /// <summary>
    /// ISO 4217 currency code (e.g., USD, GBP, JPY).
    /// </summary>
    [XmlAttribute("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// The exchange rate value.
    /// Represents how many units of the foreign currency equal one EUR.
    /// Example: rate="1.1492" means 1 EUR = 1.1492 USD
    /// </summary>
    [XmlAttribute("rate")]
    public decimal Rate { get; set; }
}
