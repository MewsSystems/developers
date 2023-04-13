using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class CzechExchangeRateProvider : IExchangeRateProvider
    {
        private const string TARGET_CURRENCY = "CZK";
        private readonly IExchangeRateFetcher _fetcher;
        private readonly IExchangeRateParser _parser;
        private readonly IClock _clock;


        public CzechExchangeRateProvider(IExchangeRateFetcher fetcher, IExchangeRateParser parser, IClock clock)
        {
            _fetcher = fetcher;
            _parser = parser;
            _clock = clock;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies is null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            // Check daily exchange rates for all currencies
            var cnbHtml = await _fetcher.FetchDailyExchangeRateData(_clock.Today);
            var exchangeRates = _parser.ParseExchangeRates(cnbHtml, new Currency(TARGET_CURRENCY), currencies).ToList();

            // If all currencies are found, return the exchange rates
            var missingCurrencies = currencies.Except(exchangeRates.Select(x => x.SourceCurrency)).ToList();
            if (!missingCurrencies.Any())
                return exchangeRates;

            // Check monthly exchange rates for any missing currencies
            cnbHtml = await _fetcher.FetchMonthlyExchangeRateData(_clock.Today);
            exchangeRates.AddRange(_parser.ParseExchangeRates(cnbHtml, new Currency(TARGET_CURRENCY), missingCurrencies));

            return exchangeRates;
        }
    }
}
