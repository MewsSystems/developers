using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateProvider
    {
        readonly ExchangeRateProviderOptions options;

        public ExchangeRateProvider(IOptions<ExchangeRateProviderOptions> options)
        {
            this.options = options.Value;
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
            var response = await DownloadExhangeRatesContent(cancellation);
            using var responseReader = new StringReader(response);
            // Throw away the first response line with date and ID.
            responseReader.ReadLine(); 
            // the rest of the file is a CSV with | as a separator and Czech culture for number styles.
            using var csv = new CsvReader(responseReader,
                new CsvConfiguration(CultureInfo.GetCultureInfo("cs-CZ")) { Delimiter = "|" });
            var parsedRates = csv.GetRecords<ParsedRate>();
            // We need to materialize the list because the csv reader will get disposed.
            return FilterAndMapRates(parsedRates, currencies).ToList();
        }

        private IEnumerable<ExchangeRate> FilterAndMapRates(IEnumerable<ParsedRate> parsedRates,
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

        private async Task<string> DownloadExhangeRatesContent(CancellationToken cancellation)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(options.DownloadUri, cancellation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        class ParsedRate
        {
            [Index(2)] public int Amount { get; set; }
            [Index(3)] public string Code { get; set; } = "";
            [Index(4)] public decimal Rate { get; set; }
        };
    }

    public sealed record ExchangeRateProviderOptions(Uri? downloadUri = default, TimeSpan? downloadTimeout = default)
    {
        public Uri DownloadUri { get; init; } = downloadUri ??
            new Uri("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt");
    }
}
