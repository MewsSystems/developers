namespace ExchangeRateUpdater.Models;

/// <summary>
///     The response of the daily exchange rates endpoint
/// </summary>
/// <param name="Rates">The actual exchange rates</param>
public record GetDailyExchangeRatesResponse(IEnumerable<GetDailyExchangeRatesResponseItem> Rates);