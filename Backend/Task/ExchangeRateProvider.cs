using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        private HttpClient httpClient;
        private IExchangeRatesParser exchangeRateParser;
        public string FxRatesUrl { get; }

        // "Injecting" the parser in the constructor because I wanted to make it (parsing of a concrete format) more versatile and extendable.
        // This way it should be easier to separate the code concerned with parsing the response from the "business" logic (i.e., the filtering)
        public ExchangeRateProvider(HttpClient httpClient, string fxRatesUrl, IExchangeRatesParser exchangeRateParser)
        {
            this.httpClient = httpClient;
            this.exchangeRateParser = exchangeRateParser;
            FxRatesUrl = fxRatesUrl;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {      
            var fxRatesRaw = string.Empty;
            IEnumerable<ExchangeRate> fxRatesParsed;
            IEnumerable<ExchangeRate> fxFiltered;
                
            try
            {
                fxRatesRaw = await httpClient.GetStringAsync(FxRatesUrl);
            }
            catch (HttpRequestException e)
            {
                // e.g. log the exception
                throw;
            }

            
            fxRatesParsed = exchangeRateParser.ParseExchangeRates(fxRatesRaw);

            fxFiltered = fxRatesParsed.Where(r => currencies.Contains(r.SourceCurrency));  
           
            return fxFiltered;
        }
    }
}
