using MewsFinance.Domain.Models;

namespace MewsFinance.Application.Extensions
{
    public static class ExchangeRateEnumerableExtension
    {
        public static IEnumerable<ExchangeRate> FilterBySourceCurrencyAndUnitAmount(this IEnumerable<ExchangeRate> exchangeRates, IEnumerable<string> sourceCodes)
        {
            var filteredExchangeRates = exchangeRates.Where(r => sourceCodes.Contains(r.SourceCurrency.Code) && r.IsCurrencyUnitAmount);

            return filteredExchangeRates;
        }
    }
}