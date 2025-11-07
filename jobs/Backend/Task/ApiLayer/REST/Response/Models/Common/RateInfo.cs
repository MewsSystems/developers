namespace REST.Response.Models.Common;

/// <summary>
/// Exchange rate details nested object for API responses.
/// </summary>
public class RateInfo
{
    /// <summary>
    /// Exchange rate value.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Multiplier for the rate (typically 1, but could be 100 for currencies like JPY).
    /// </summary>
    public int Multiplier { get; set; }

    /// <summary>
    /// Effective rate after applying the multiplier.
    /// This is the actual conversion rate to use: 1 Base = EffectiveRate Target.
    /// </summary>
    public decimal EffectiveRate { get; set; }
}
