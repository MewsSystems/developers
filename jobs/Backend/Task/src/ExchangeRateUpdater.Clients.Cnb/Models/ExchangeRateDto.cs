namespace ExchangeRateUpdater.Clients.Cnb.Models;

/// <summary>
/// The Dto of Exchange rate response
/// </summary>
public class ExchangeRateDto
{
    /// <summary>
    /// The Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// The Currency
    /// </summary>
    public string? Currency { get; set; }

    /// <summary>
    /// The Amount
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The Code
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The Rate
    /// </summary>
    public decimal Rate { get; set; }
}