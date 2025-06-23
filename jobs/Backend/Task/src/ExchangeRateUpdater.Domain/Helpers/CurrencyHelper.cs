using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Domain.Helpers
{
    public static class CurrencyHelper
    {
        public static IReadOnlyCollection<Currency> GenerateCurrencies(params string[] currencyCodes)
        {
            return currencyCodes.Select(code => new Currency(code)).ToList();
        }
    }
}
