namespace Mews.ExchangeRateProvider;

/// <summary>
/// Obtains exchange rate information
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Obtains the most recent available exchange rate data for a given list of currencies
    /// </summary>
    /// <param name="currencies">A list of currency pairs to obtain exchange rate data for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A Task that will return a collection of <see cref="ExchangeRate"/> objects</returns>
    /// <exception cref="ObtainExchangeRateException">Thrown if there is a issue obtaining exchange rate data</exception>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}