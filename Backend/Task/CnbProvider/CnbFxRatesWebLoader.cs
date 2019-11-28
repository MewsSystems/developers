using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace ExchangeRateUpdater.CnbProvider
{
    internal interface ICnbFxRatesWebLoader
    {
        /// <summary>
        /// Will try to get rates from source if newer version was published
        /// </summary>
        /// <returns>Rates where updated</returns>
        Task<bool> TryGetLatestRates(CnbFxRatesSource source);
    }

    class CnbFxRatesWebLoader : ICnbFxRatesWebLoader
    {
        private static readonly (int retries, TimeSpan waitTime, TimeSpan timeout) PolicyConfig = (retries: 3, waitTime: TimeSpan.FromSeconds(10), timeout: TimeSpan.FromMinutes(1));
        private static readonly HttpClient HttpClient =  new HttpClient { Timeout = PolicyConfig.timeout };
    
        private readonly ICnbFxRateRowParser _cnbRateParser;

        public CnbFxRatesWebLoader(ICnbFxRateRowParser cnbFxRateRowParser)
        {
            _cnbRateParser = cnbFxRateRowParser ?? throw new ArgumentNullException(nameof(cnbFxRateRowParser));
        }

        public async Task<bool> TryGetLatestRates(CnbFxRatesSource source)
        {
            var query = source.CreateQueryString();

            //external web source, we need to handle outages
            var policy = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(PolicyConfig.retries, wait => PolicyConfig.waitTime);

            using (var streamReader = new StreamReader(await policy.ExecuteAsync(()=> HttpClient.GetStreamAsync($"{source.Url}?{query}")).ConfigureAwait(false)))
            {
                if (streamReader.EndOfStream)
                    return false;

                var versionRow = await streamReader.ReadLineAsync();

                if (source.VersionsMatch(versionRow))
                    return false;

                await SkipHeader(streamReader);

                source.Current = new CnbFxRatesSource.Context(newRates: await ParseAllRates(streamReader), usedQueryString: query, versionRow: versionRow);
            }

            return true;
        }

        private static async Task SkipHeader(StreamReader streamReader)
        {
            if (!streamReader.EndOfStream) await streamReader.ReadLineAsync();
        }

        private async Task<List<ExchangeRate>> ParseAllRates(StreamReader streamReader)
        {
            var result = new List<ExchangeRate>();
            while (!streamReader.EndOfStream)
            {
                var rate = _cnbRateParser.ParseRateRow(await streamReader.ReadLineAsync());
                if (rate != null) result.Add(rate);
            }

            return result;
        }
    }
}