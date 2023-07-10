using ExchangeRateUpdater.Models.Types;
namespace ExchangeRateUpdater.Models.Behavior;
internal static class CurrencySerialization
{
    /// <summary>
    /// Converts a <see cref="Currency"/> object to its string representation.
    /// </summary>
    /// <param name="currency">The <see cref="Currency"/> object to convert.</param>
    /// <returns>A string representing the <see cref="Currency"/> object.</returns>
    internal static string ToStringFormat(this Currency currency) => currency.Code.Value;
}