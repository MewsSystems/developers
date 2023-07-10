using ExchangeRateUpdater.Models.Types;

namespace ExchangeRateUpdater.Models.Behavior;
internal static class ExchangeRateSerialization
{
    /// <summary>
    /// Returns the required string representation of an ExchangeRate item, i.e. "EUR/CZK=23.945"
    /// </summary>
    /// <param name="exchangeRate"></param>
    internal static string ToStringFormat(this ExchangeRate exchangeRate) =>
        $"{exchangeRate.SourceCurrency.Code.Value}/{exchangeRate.TargetCurrency.Code.Value}={exchangeRate.Rate.Value}";
}