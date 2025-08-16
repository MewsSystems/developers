using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;

namespace Mews.ExchangeRateProvider.Application.Utils
{
    public static class ValidCurrenciesList
    {
        // use this list to filter values from CNB endpoint to this currencies if getAllRates is false
        public static IEnumerable<Currency> currencies = new[]
       {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
    }
}
