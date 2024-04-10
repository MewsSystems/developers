using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Models;

namespace ExchangeRates.Infrastructure.Mappers
{
    internal class ExchangeRateMapper : IExchangeRateMapper
    {
        public IEnumerable<ExchangeRate> Map(ExRateDailyResponse exRateDailyResponse)
        {
            var exchangeRates = new List<ExchangeRate>();

            foreach (var sourceRate in exRateDailyResponse.ExRates)
            {
                foreach (var targetRate in exRateDailyResponse.ExRates)
                {
                    if (sourceRate.CurrencyCode == targetRate.CurrencyCode)
                    {
                        continue;
                    }

                    exchangeRates.Add(new ExchangeRate(
                        new Currency(sourceRate.CurrencyCode),
                        new Currency(targetRate.CurrencyCode),
                        Math.Round(targetRate.Rate / sourceRate.Rate, 3)));
                }
            }

            return exchangeRates;
        }
    }
}
