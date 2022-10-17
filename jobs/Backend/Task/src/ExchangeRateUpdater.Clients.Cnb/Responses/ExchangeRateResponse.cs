namespace ExchangeRateUpdater.Clients.Cnb.Responses;

/// <summary>
/// The response of the cnb client.
/// </summary>
public class ExchangeRateResponse
{
    /// <summary>
    /// The exchange rates.
    /// </summary>
    public List<ExchangeRateDto?> ExchangeRates { get; init; } = new();
    
    /// <summary>
    /// The current date
    /// </summary>
    public DateTime CurrentDate { get; set; }
}