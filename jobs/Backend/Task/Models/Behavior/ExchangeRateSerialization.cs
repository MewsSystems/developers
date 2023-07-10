using ExchangeRateUpdater.Models.Types;

namespace ExchangeRateUpdater.Models.Behavior;
internal static class ExchangeRateSerialization
{
    /// <summary>
    /// Converts an <see cref="ExchangeRate"/> object to its string representation.
    /// </summary>
    /// <param name="exchangeRate">The <see cref="ExchangeRate"/> object to convert.</param>
    /// <returns>A string representing the <see cref="ExchangeRate"/> object, i.e. "EUR/CZK=23.945"</returns>
    internal static string ToStringFormat(this ExchangeRate exchangeRate) =>
        $"{exchangeRate.SourceCurrency.Code.Value}/{exchangeRate.TargetCurrency.Code.Value}={exchangeRate.Rate.Value}";
}