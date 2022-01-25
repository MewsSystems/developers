using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.QuotesProviders
{
    public class CnbQuotesHttpProvider : IQuotesProvider
    {
        
        private readonly IWebProxyProvider _webProxyProvider;
        public CnbQuotesHttpProvider(IWebProxyProvider webProxyProvider)
        {
            _webProxyProvider = webProxyProvider;
        }

        public static string ProviderUrl => "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        public async Task<string> GetQuotesAsync() => await _webProxyProvider.GetUrlAsync(ProviderUrl);

    }
}
