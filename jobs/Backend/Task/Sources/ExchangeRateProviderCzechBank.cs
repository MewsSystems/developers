using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ExchangeRateUpdater.Sources
{
    /// <summary>
    /// Task would be more interesting if 'extra' requirement would involve storing historical data, caching etc. 
    /// I can add it ofc, but I don't want to over complicate the requirements. 
    /// </summary>
    public class ExchangeRateProviderCzechBank : IExchangeRateProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IExchangeRateParser _parser;
        
        public ExchangeRateProviderCzechBank(IConfiguration configuration, IExchangeRateParser parser)
        {
            _configuration = configuration;
            _parser = parser;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, string data = null)
        {
            var content = data ?? await GetResponse();
            var lines = content?.Replace("\r\n", "\n").Split('\n') ?? Array.Empty<string>();

            var currenciesArray = currencies.ToArray();
            return lines.Where(e => !string.IsNullOrWhiteSpace(e))
                        .Select(e => _parser.ParseExchangeRate(e))
                        .Where(e => e.Success)
                        .Select(e => e.Rate)
                        .Where(e => !currenciesArray.Any() /* return all on empty */ 
                                    || currenciesArray.Any(c => c.Code.Equals(e.TargetCurrency.Code, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<string> GetResponse()
        {
            var client = new RestClient();
            var request = new RestRequest($"{_configuration["ExchangeRatesUrl"]}?date={DateTimeOffset.Now:dd.MM.yyyy}");
            var response = await client.ExecuteAsync(request);

            var content = response.Content;
            return content;
        }
    }
}
