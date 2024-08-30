using ExchangeRateUpdater.Services.Client;
using ExchangeRateUpdater.Services.Client.ClientModel;
using ExchangeRateUpdater.Services.Configuration;
using ExchangeRateUpdater.Services.Domain;
using ExchangeRateUpdater.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const int _longTermCacheExpiry = 2;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly AppConfiguration _configuration;
        private readonly ICzechNationalBankClient _czechNationalBankClient;
        private readonly IMemoryCache _cache;

        public ExchangeRateProvider(
            ILogger<ExchangeRateProvider> logger,
            AppConfiguration configuration,
            ICzechNationalBankClient czechNationalBankClient,
            IMemoryCache cache)
        {
            _logger = logger;
            _configuration = configuration;
            _czechNationalBankClient = czechNationalBankClient;
            _cache = cache;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return
                (await GetDailyRatesAsync())
                    .Rates
                    .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
                    .Select(x => (ExchangeRate)x);
        }


        private async Task<ExchangeRateResponseList> GetDailyRatesAsync()
        {
            if (!_cache.TryGetValue("DailyRatesShortTerm", out ExchangeRateResponseList? rates))
            {
                try
                {
                    rates = await _czechNationalBankClient.GetDailyRatesAsync(_configuration.Culture).ConfigureAwait(false);

                    TimeSpan cacheExpiryShortTerm = GetTimeDifference();

                    _cache.Set("DailyRatesShortTerm", rates, cacheExpiryShortTerm);

                    // if for some reason we fail to get the rates from the API (if the API is down temporarily) we will keep it for a longer
                    _cache.Set("DailyRatesLongTerm", rates, TimeSpan.FromDays(_longTermCacheExpiry));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting rates from API");

                    // If there's an error getting the rates from the API, use the old rates from the long-term cache if available
                    _cache.TryGetValue("DailyRatesLongTerm", out rates);
                }
            }

            //if after 2 days we can't still get the rates, then we will return an empty list and keep the log above. After 2 days, it is too long to rely on accuracy
            return rates ?? new ExchangeRateResponseList { Rates = new List<ExchangeRateResponse>() };
        }

        private TimeSpan GetTimeDifference()
        {
            var localTime = DateTime.Now;

            var pragueTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var pragueTime = TimeZoneInfo.ConvertTime(localTime, pragueTimeZone);

            // Get next 14:30 Prague time. Assuming it's before 14:30, get 14:30 of the current day
            var nextPrague1430 = pragueTime.Date.AddHours(14).AddMinutes(30);
            if (pragueTime.TimeOfDay >= new TimeSpan(14, 30, 0))
            {
                // If it's already past 14:30, get 14:30 of the next day
                nextPrague1430 = pragueTime.Date.AddDays(1).AddHours(14).AddMinutes(30);
            }

            return nextPrague1430 - pragueTime;
        }
    }
}
