namespace ExchangeRateUpdater.Host.WebApi.Dtos.Response;

/// <summary>
/// The Dto representing the exchange rate.
/// </summary>
/// <remarks>All exchange rates are of a 1:1 ratio.</remarks>
internal class ExchangeRateDto
{
    /// <summary>
    /// The code of the currency which the conversion happens from.
    /// </summary>
    public string? From { get; set; }
    /// <summary>
    /// The code of the currency which the conversion happens to.
    /// </summary>
    public string? To { get; set; }
    /// <summary>
    /// Exchange rate specfied for a 1:1 ratio.
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// The time of the exchange rate.
    /// </summary>
    public DateTime ExchangeRateTime { get; set; }
}
