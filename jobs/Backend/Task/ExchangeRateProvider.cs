using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateProvider
    {
        readonly IExchangeRateDownloader downloader;
        readonly ExchangeRateParser parser;

        public ExchangeRateProvider(
            IExchangeRateDownloader downloader,
            ExchangeRateParser parser)
        {
            this.downloader = downloader;
            this.parser = parser;
        }
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(
            IEnumerable<Currency> currencies, CancellationToken cancellation)
        {
            var response = await downloader.DownloadExchangeRatesAsync(cancellation);
            var parsedRates = parser.ParseRates(response);
            return FilterAndMapRates(parsedRates, currencies);
        }

        private IEnumerable<ExchangeRate> FilterAndMapRates(IEnumerable<ExchangeRateParser.ParsedRate> parsedRates,
            IEnumerable<Currency> requestedCurrencies)
        {
            // CNB rates assume CZK as source currency for all rates
            var sourceCurrency = new Currency("CZK");
            // Prepare string set for faster filtering, but with the number of currencies bounded by real world this
            // may be pointless with current CPU cache line sizes - further perf opt would need proper benchmarking.
            var currencyFilter = new HashSet<string>(requestedCurrencies.Select(c => c.Code));
            // Take only the requested rates and calculate the result. The downloaded rates are not always 1:1,
            // we need to take that into account by dividing by Amount (of the source currency).
            return parsedRates
                .Where(r => currencyFilter.Contains(r.Code)) // .NET 6 will finally bring IntersectBy(source, keys, selector)
                .Select(r => new ExchangeRate(sourceCurrency, new Currency(r.Code), r.Rate / r.Amount));
        }
    }

    public interface IExchangeRateDownloader
    {
        Task<string> DownloadExchangeRatesAsync(CancellationToken cancellation);
    }
}
