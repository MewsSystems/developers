namespace ExchangeRateUpdater.Contracts;

/// <summary>
///     Represents an interface with an external banking API that offers daily exchange rates data
/// </summary>
public interface IExternalBankApiClient
{
    /// <summary>
    ///     Gets the daily exchange rates
    /// </summary>
    /// <param name="date">The reference date</param>
    /// <param name="lang">The language of currency descriptions</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The daily exchange rates</returns>
    Task<GetDailyExchangeRatesResponse> GetDailyExchangeRatesAsync(DateOnly? date = null, string? lang = null,
        CancellationToken cancellationToken = default);
}