using System.Xml.Serialization;

namespace CzechNationalBank.Models;

/// <summary>
/// Represents the root element of the CNB (Czech National Bank) XML response.
/// The CNB provides exchange rates via XML at: https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml
/// XML Structure: kurzy/tabulka/radek with namespace http://www.cnb.cz/xsd/Filharmonie/modely/Denni_kurz/1.1
/// </summary>
[XmlRoot("kurzy")]
public class CnbExchangeRates
{
    /// <summary>
    /// The type of exchange rates (e.g., "devízy", "bankovky")
    /// </summary>
    [XmlAttribute("typ")]
    public string? Type { get; set; }

    /// <summary>
    /// The bank name (should be "CNB")
    /// </summary>
    [XmlAttribute("banka")]
    public string? Bank { get; set; }

    /// <summary>
    /// The date of the exchange rates (format: DD.MM.YYYY)
    /// </summary>
    [XmlAttribute("datum")]
    public string? Date { get; set; }

    /// <summary>
    /// Serial number of the daily rates
    /// </summary>
    [XmlAttribute("poradi")]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Table wrapper containing the list of exchange rates
    /// </summary>
    [XmlElement("tabulka")]
    public CnbTable? Table { get; set; }
}

/// <summary>
/// Represents the table wrapper element containing exchange rates.
/// </summary>
public class CnbTable
{
    /// <summary>
    /// List of individual exchange rates for different currencies
    /// </summary>
    [XmlElement("radek")]
    public List<CnbRate> Rates { get; set; } = new();
}

/// <summary>
/// Represents an individual exchange rate entry.
/// Format: [amount] [currency] = [value] CZK
/// Example: 1 USD = 22.456 CZK
/// </summary>
public class CnbRate
{
    /// <summary>
    /// Country name in Czech (e.g., "USA", "Austrálie")
    /// </summary>
    [XmlAttribute("zeme")]
    public string? Country { get; set; }

    /// <summary>
    /// Currency name in Czech (e.g., "dolar", "peso")
    /// </summary>
    [XmlAttribute("mena")]
    public string? Currency { get; set; }

    /// <summary>
    /// The amount/multiplier of the foreign currency
    /// Usually 1, but some currencies use 100, 1000, etc.
    /// </summary>
    [XmlAttribute("mnozstvi")]
    public int Amount { get; set; }

    /// <summary>
    /// ISO 4217 currency code (e.g., USD, EUR, GBP)
    /// </summary>
    [XmlAttribute("kod")]
    public string? Code { get; set; }

    /// <summary>
    /// The exchange rate value in CZK
    /// Represents how many CZK for the given amount of foreign currency
    /// </summary>
    [XmlAttribute("kurz")]
    public string? RateValue { get; set; }
}
