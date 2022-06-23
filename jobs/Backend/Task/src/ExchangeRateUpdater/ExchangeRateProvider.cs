using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models.Entities;
using ExchangeRateUpdater.Service.Cnb;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly CnbService _service;
        private readonly string _timezone;

        public ExchangeRateProvider(ExchangeRateProviderSettings settings)
        {
            _timezone = settings.TimezoneId;
            _service  = new CnbService(new HttpClient(), 
                                       settings.BaseUrl,
                                       settings.DefaultCurrency,
                                       settings.MappingDelimiter,
                                       settings.MappingDecimalSeparator,
                                       settings.ThrowExceptionOnMappingErrors);
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var currentTime = DateTime.UtcNow.WithTimezone(_timezone);
            var rates = await _service.GetExchangeRatesAsync(currentTime);

            var filteredRates = rates.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));

            return filteredRates;
        }
    }
}