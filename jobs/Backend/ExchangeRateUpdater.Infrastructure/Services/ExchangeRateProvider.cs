using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Shared;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.Extensions;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using ExchangeRateUpdater.Infrastructure.Services.Interfaces;
using ExchangeRateUpdater.Infrastructure.Services.Helpers;

namespace ExchangeRateUpdater.Infrastructure.Services
{
    public class ExchangeRateProvider(IOptions<Settings> settings, ExchangeRateCacheManager exchangeRateCacheManager) : IExchangeRateProvider
    {
        private readonly Settings _settings = settings.Value;
        private readonly Currency targetCurrency = new(settings.Value.DefaultCurrency);
        private readonly ExchangeRateCacheManager _cacheManager = exchangeRateCacheManager;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public Result<IEnumerable<ExchangeRate>> GetExchangeRates(string scope, string? date = null, string? currency = null, IEnumerable<Currency>? currencies = null)
        {            
            if (!ValidationHelper.ValidateDateFormat(scope,ref date, out var formatException) ||
                !ValidationHelper.ValidateCurrency(currency, out formatException) ||
                !ValidationHelper.ValidateCurrency(currencies, out formatException))
                return new(formatException);

            var exchangeRates = _cacheManager.GetDailyRates(date, CurrentUrl(scope, date, currency));

            return exchangeRates.Match<Result<IEnumerable<ExchangeRate>>>(
                x => x.FilterByCurrencies(currencies).ToExchangeRates(scope, targetCurrency).ToList(),
                ex => new(ex));
        }

        /// <summary>
        /// Constructs the URL to retrieve exchange rates from the source, based on the scope and parameters.
        /// </summary>
        /// <param name="scope">The scope or type of exchange rates (e.g., daily, daily-year, daily-currency-month).</param>
        /// <param name="date">The date or year-month combination for the exchange rate query.</param>
        /// <param name="currency">The base currency for the query.</param>
        /// <returns>A formatted URL string that can be used to retrieve exchange rates from the source.</returns>
        private string CurrentUrl(string scope, string date, string? currency) => scope switch
        {
            ExchangeRateRoutes.Daily => $"{_settings.CnbUrl}/{scope}?date={date}",
            ExchangeRateRoutes.DailyYear => $"{_settings.CnbUrl}/{scope}?year={date}",
            ExchangeRateRoutes.DailyCurrencyMonth => $"{_settings.CnbUrl}/{scope}?currency={currency}&yearMonth={date}",
            _ => throw new ArgumentException(),
        };
    }
}
