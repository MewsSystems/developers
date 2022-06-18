using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider 
    {
        private readonly HttpClient _httpClient;
        private readonly string _sourceUrl;

        public CnbExchangeRateProvider(HttpClient httpClient, string sourceUrl)
        {
            _httpClient = httpClient;
            _sourceUrl = sourceUrl;
        }

        public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await _httpClient.GetAsync(_sourceUrl);

            var stream = await response.Content.ReadAsStreamAsync();

            return Result.Ok(Enumerable.Empty<ExchangeRate>());
        }
    }
}
