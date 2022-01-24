using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    /// <summary>
    /// Czech National Bank provider 
    /// <see cref="https://www.cnb.cz/en/"/>
    /// </summary>
    public class CnbExchangeRateProvider : ExchangeRateProvider
    {
        
        private static RegionInfo CzechRepublicRegion = new RegionInfo("cs-CZ");
        private static Currency BaseCurrency = new Currency(CzechRepublicRegion.ISOCurrencySymbol);


        private readonly IQuotesProvider _quotesProvider;
        private readonly IQuotesParser _quotesParser;

        public CnbExchangeRateProvider(IQuotesProvider quotesProvider, IQuotesParser quotesParser)
        {
            _quotesProvider = quotesProvider;
            _quotesParser = quotesParser;
        }

        public override async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var rates = await _quotesProvider.GetQuotesAsync();
            var parsedRates = _quotesParser.ParseQuotes(BaseCurrency, rates);

            return currencies
                .Where(curr => parsedRates.ContainsKey(curr))
                .Select(quote => parsedRates[quote])
                .ToList();
        }
    }
}
