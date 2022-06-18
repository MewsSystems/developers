using ExchangeRateUpdated.Service.Parsers;
using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider 
    {
        private readonly HttpClient _httpClient;
        private readonly string _sourceUrl;
        private readonly ICnbCsvParser _cnbCsvParser;

        public CnbExchangeRateProvider(HttpClient httpClient, string sourceUrl, ICnbCsvParser cnbCsvParser)
        {
            _httpClient = httpClient;
            _sourceUrl = sourceUrl;
            _cnbCsvParser = cnbCsvParser;
        }

        public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await _httpClient.GetAsync(_sourceUrl);

            var stream = await response.Content.ReadAsStreamAsync();

            var result = _cnbCsvParser.TryParseExchangeRates(stream);

            return result switch
            {
                { IsFailed: true } => Result.Fail(result.Errors),
                { IsSuccess: true } => Result.Ok(result.Value.Select(c => new ExchangeRate("CZK", c.Code, c.Rate)))
            };
        }
    }
}
