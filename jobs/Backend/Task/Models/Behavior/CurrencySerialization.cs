using ExchangeRateUpdater.Models.Types;
namespace ExchangeRateUpdater.Models.Behavior;
internal static class CurrencySerialization
{
    internal static string ToStringFormat(this Currency currency) => currency.Code.Value;
}
