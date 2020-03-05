using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    ///     Exchange rate provider for CNB. Learn more about refresh period of exchange rates at
    ///     https://www.cnb.cz/cs/casto-kladene-dotazy/Kurzy-devizoveho-trhu-na-www-strankach-CNB/.
    ///     Configure of provider with <see cref="ExchangeRateProviderConfiguration"/>.
    /// </summary>
    public class ExchangeRateProvider : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _disposeClient;
        private readonly ExchangeRateSourceParser _parser;
        private readonly ExchangeRateProviderPrimaryDailySourceConfiguration _primarySourceConfiguration;
        private readonly ExchangeRateProviderSecondaryMonthlySourceConfiguration _secondarySourceConfiguration;

        /// <summary>
        ///     Initializes new disposable instance of the <see cref="ExchangeRateProvider"/>.
        ///     Accepts optional <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        public ExchangeRateProvider(HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _disposeClient = httpClient is null;
            
            var section = (ExchangeRateProviderConfiguration) ConfigurationManager.GetSection(nameof(ExchangeRateProviderConfiguration));
            _primarySourceConfiguration = section.PrimaryDailyExchangeRateSource;
            _secondarySourceConfiguration = section.SecondaryMonthlyExchangeRateSource;
            
            _parser = new ExchangeRateSourceParser(new Currency("CZK"));
        }

        /// <summary>
        ///     Retrieve exchange rates for selected currencies with CZK as target currency.
        /// </summary>
        /// <param name="currencies">The source currencies.</param>
        /// <returns>The exchange rates for source currencies.</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return GetExchangeRatesAsync(currencies).GetAwaiter().GetResult();
        }
        
        /// <summary>
        ///     Retrieve exchange rates for selected currencies with CZK as target currency.
        /// </summary>
        /// <param name="currencies">The source currencies.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The exchange rates for source currencies.</returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (currencies is null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            var primaryDailyExchangeRates = await GetExchangeRatesFromSource(_primarySourceConfiguration.Url, cancellationToken);
            var secondaryMonthlyExchangeRates = await GetExchangeRatesFromSource(_secondarySourceConfiguration.Url, cancellationToken);

            return primaryDailyExchangeRates
                .Concat(secondaryMonthlyExchangeRates)
                .Where(exRate => currencies.Any(currency => currency.Code == exRate.SourceCurrency.Code));
        }
        
        public void Dispose()
        {
            if (_disposeClient)
            {
                _httpClient.Dispose();
            }
        }
        
        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromSource(string sourceUrl, 
            CancellationToken cancellationToken)
        {
            var httpResult = await _httpClient.GetAsync(sourceUrl, cancellationToken);
            if (!httpResult.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to retrieve data from exchange rate source ({sourceUrl}) with status code {httpResult.StatusCode}.");

            var content = await httpResult.Content.ReadAsStringAsync();
            
            return await _parser.ParseExchangeRatesAsync(content);
        }
    }
}
