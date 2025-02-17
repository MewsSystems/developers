using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Domain.DTOs;

/// <summary>
///     Represents the result of an exchange rate query.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ExchangeRateResult" /> class.
/// </remarks>
/// <param name="validRates">A collection of successfully resolved exchange rates.</param>
/// <param name="invalidCurrencies">A collection of currencies that could not be resolved.</param>
public class ExchangeRateResult(IEnumerable<ExchangeRate> validRates, IEnumerable<string> invalidCurrencies)
{
    /// <summary>
    ///     Gets the list of successfully resolved exchange rates.
    /// </summary>
    public IEnumerable<ExchangeRate> Rates { get; } = validRates;

    /// <summary>
    ///     Gets the list of currencies that could not be resolved.
    /// </summary>
    public IEnumerable<string> CurrenciesNotResolved { get; } = invalidCurrencies;
}
