using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateSource
    {
        private readonly string _url;

        public CnbExchangeRateSource(Options options)
        {
            _url = options.Url ?? throw new ArgumentNullException(nameof(Options.Url));
        }

        public async Task<string> GetLatestExchangeRatesAsync()
        {
            var content = await _url.GetStringAsync();
            return content;
        }

        public record Options
        {
            public string Url { get; set; }
        }
    }
}
