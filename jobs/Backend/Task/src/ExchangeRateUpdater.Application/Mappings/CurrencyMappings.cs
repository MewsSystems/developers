using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Application.Mappings
{
    internal static class CurrencyMappings
    {
        internal static IEnumerable<Currency> Map(IEnumerable<CurrencyModel> currenciesModel)
        {
            var currencies = new List<Currency>();
            currenciesModel.ToList().ForEach(c => currencies.Add(new Currency(c.Code)));
            return currencies;
        }

    }
}
