using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string TargetCurrency = "CZK";
        private readonly IExchangeRateFetcher _fetcher;
        private readonly IExchangeRateParser _parser;

        public ExchangeRateProvider(IExchangeRateFetcher fetcher, IExchangeRateParser parser)
        {
            _fetcher = fetcher;
            _parser = parser;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var cnbHtml = await _fetcher.FetchExchangeRateData();
            return _parser.ParseExchangeRates(cnbHtml, new Currency(TargetCurrency), currencies);
        }
    }
}
