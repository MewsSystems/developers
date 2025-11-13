using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Infrastucture.Data.API.Entities;

namespace ExchangeRateUpdater.Application.Mappings
{
    internal static class ExchangeRateMappings
    {
        internal static IEnumerable<ExchangeRate> Map(RatesDTO ratesDTO, string sourceCurrency)
        {
            var currencies = new List<ExchangeRate>();
            ratesDTO.Rates.ToList().ForEach(c => currencies.Add(new ExchangeRate(new Currency(sourceCurrency), new Currency(c.CurrencyCode), c.Rate)));
            return currencies;
        }

        internal static ExchangeRateModel Map(IEnumerable<ExchangeRate> exchangeRate)
        {
            var exchangeRateModel = new ExchangeRateModel();
            exchangeRate.ToList().ForEach(c => exchangeRateModel.Add(c.ToString()));
            return exchangeRateModel;
        }
    }
}
