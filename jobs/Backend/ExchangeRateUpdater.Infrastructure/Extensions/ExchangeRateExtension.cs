using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Extensions
{
    public static class ExchangeRateExtension
    {
        public static IEnumerable<ExchangeRateRow> FilterByCurrencies(this IEnumerable<ExchangeRateRow> exchangeRateRows, IEnumerable<Currency>? currencies) =>
            currencies is not null && currencies.Any() ?
                exchangeRateRows.Where(x => currencies.Any(z => z.ToString().Equals(x.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))) : exchangeRateRows;

        public static IEnumerable<ExchangeRate> ToExchangeRates(this IEnumerable<ExchangeRateRow> exchangeRateRows, string scope, Currency targetCurrency) =>
            scope switch
            {
                ExchangeRateRoutes.Daily => exchangeRateRows.Select(x => new DailyExchangeRate(new(x.CurrencyCode), targetCurrency, x.Rate / x.Amount)).Cast<ExchangeRate>().ToList(),
                ExchangeRateRoutes.DailyCurrencyMonth or ExchangeRateRoutes.DailyYear => exchangeRateRows.Select(x => new MonthYearExchangeRate(new(x.CurrencyCode), targetCurrency, x.Rate / x.Amount, x.ValidFor)).Cast<ExchangeRate>().ToList(),
                _ => throw new InvalidOperationException()
            };

        public static IEnumerable<Currency> ToCurrency(this string[] currencies) => currencies.Select(currency => new Currency(currency));

    }
}
