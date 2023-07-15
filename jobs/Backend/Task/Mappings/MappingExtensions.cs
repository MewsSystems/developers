using ExchangeRateUpdater.ApiClients.Responses;
using ExchangeRateUpdater.Models.Behavior;
using ExchangeRateUpdater.Models.Types;

namespace ExchangeRateUpdater.Mappings;

/// <summary>
/// Contains extension methods for mapping objects.
/// </summary>
internal static class MappingExtensions
{
    /// <summary>
    /// Converts an <see cref="ExchangeRateApiItem"/> object to an <see cref="ExchangeRate"/> object.
    /// </summary>
    /// <param name="apiItem">The <see cref="ExchangeRateApiItem"/> to convert.</param>
    /// <param name="targetCurrency">The code of the target currency.</param>
    /// <returns>An <see cref="ExchangeRate"/> object representing the converted exchange rate.</returns>
    internal static ExchangeRate ToExchangeRateResult(this ExchangeRateApiItem apiItem, string targetCurrency) =>
            new(SourceCurrency: new Currency(new Code(apiItem.CurrencyCode)), 
                TargetCurrency: new Currency(new Code(targetCurrency)), 
                Rate: new Rate(apiItem.Rate).GetByAmount(apiItem.Amount));
}
