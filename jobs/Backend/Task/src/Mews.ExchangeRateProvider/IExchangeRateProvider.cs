using Mews.ExchangeRateProvider.Exceptions;

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
    /// <returns>A Task that will return a collection of <see cref="ExchangeRate"/> objects for each supplied currency, if known to the remote provider</returns>
    /// <exception cref="ArgumentNullException">Thrown if input parameters are null</exception>
    /// <exception cref="ArgumentException">Thrown if list of currencies is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown if no endpoint URIs are defined in the _options field</exception>
    /// <exception cref="ObtainExchangeRateException">Thrown if there is a issue obtaining exchange rate data</exception>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}