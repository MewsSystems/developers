namespace ExchangeRate.Infrastructure.CNB.Core.Repositories;

/// <summary>
///     Repository for Exchange Rate from CNB
/// </summary>
public interface IExchangeRateRepository
{
    /// <summary>
    ///     Gets the Exchange Rate data from a data source
    /// </summary>
    /// <returns>ExchangeRate</returns>
    Task<Models.ExchangeRate> GetExchangeRatesAsync();
}
