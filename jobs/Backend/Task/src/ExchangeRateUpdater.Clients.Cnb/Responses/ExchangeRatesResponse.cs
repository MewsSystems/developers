namespace ExchangeRateUpdater.Clients.Cnb.Responses;

/// <summary>
/// The response of the cnb client.
/// </summary>
public class ExchangeRatesResponse
{
    /// <summary>
    /// The exchange rates.
    /// </summary>
    public List<ExchangeRateDto> ExchangeRates { get; } = new();
    
    /// <summary>
    /// The current date
    /// </summary>
    public DateTime CurrentDate { get; set; }
}