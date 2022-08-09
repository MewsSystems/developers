using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private Uri defaultProviderURI = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        private Uri exchangeRateSourceURI;

        /// <summary>
        /// List of available parsers define what Exchange Rate Provider is allowed to work with. At the moment, hardcoded.
        /// Note: Can be easily changed in future via dependency injection.
        /// </summary>
        private readonly List<IRateParser> availableParsers = new List<IRateParser>{new CNBParser()};
        private IRateParser parser;

        /// <summary>
        /// ExchangerRateProvider accepts URI to be used as source for the exchange rates and based on the URI tries to find appropriate parser.
        /// In case, URI is not provided, there is a default provider specified above.
        /// </summary>
        public ExchangeRateProvider(Uri providedURI)
        {
            exchangeRateSourceURI = providedURI == null ? defaultProviderURI : providedURI;
            SetUpParser();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(currencies == null || currencies.Count() == 0)
            {
                // No currencies were specified, returning empty enumerable regardless of the fact how many rates were retrieved.
                return Enumerable.Empty<ExchangeRate>();
            }
            IEnumerable<ExchangeRate> allRates = parser.ParseSource(RetrieveSource());
            return FilterRatesByCurrencies(allRates, currencies);
        }

        /// <summary>
        /// Exchange rates are filtered by provided currencies. At the moment, the one available source of exchange rates is CNB.
        /// As a national bank it provides exchange rate where source currency is always CZK. There is reasonable assumption that other sources
        /// could use the same direction of the rate. For example, national bank for UK will use pound as source.
        /// </summary>
        private IEnumerable<ExchangeRate> FilterRatesByCurrencies(IEnumerable<ExchangeRate> allRates, IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> filteredRates = new List<ExchangeRate>();
            foreach(ExchangeRate rate in allRates)
            {
                int countOfFoundCurrencies = currencies.Where(c => c.Code == rate.TargetCurrency.Code).Count();
                if(countOfFoundCurrencies > 0)
                {
                    filteredRates.Add(rate);
                }
            }

            return filteredRates;
        }        

        /// <summary>
        /// Source data is retrieved via HTTP Client and because provider can't work without it, waiting on the result.
        /// Logs appropriate message if source is reachable, but response is empty.
        /// </summary>
        private string RetrieveSource()
        {
            HttpClient httpClient = new HttpClient();
            string result;
            try
            {
                Task<string> downloadTask = httpClient.GetStringAsync(exchangeRateSourceURI);
                result = downloadTask.GetAwaiter().GetResult();
            }
            // Provider doesn't care that much about particular error (no retry policy), putting it as inner exception.
            catch(Exception e)
            {
                throw new SourceUnavailableException("Source for Exchange Rates is unavailable or the provided Source URL doesn't point to any reachable source.", e);
            }
            // Log message for visibility and return empty result, let parser deal with that.
            // Exchange Rate Provider has no chance to determine if that is correct, maybe source stopped providing data. >>> No exception is thrown.
            if(string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine($"Retrieved empty result with Exchange Rates.");
            }
            return result;
        }

        /// <summary>
        /// Exchange Rate Provider sets up parser based on provided source, match is found through Host string and will use first found parser.
        /// </summary>
        private void SetUpParser()
        {
            foreach(IRateParser availableParser in availableParsers)
            {
                if(string.Equals(availableParser.ForHost().ToLowerInvariant(), exchangeRateSourceURI.Host.ToLowerInvariant()))
                {
                    parser = availableParser;
                    break;
                }
            }
            if(parser == null)
            {
                throw new NoParserAvailableException("There is no available parser for provided Exchange Rate Source.");
            }
        }
    }
}
