using ExchangeRateUpdated.Service.Parsers;
using FluentResults;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ExchangeRateUpdater.Tests")]

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider 
    {
        // we can use IOption<T>. I will just omit it for this excercise
        internal const string SourceUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=";
        internal const string DefaultCurrencyCode = "CZK";
        private readonly HttpClient _httpClient;
        private readonly ICnbCsvParser _cnbCsvParser;

        public CnbExchangeRateProvider(HttpClient httpClient, ICnbCsvParser cnbCsvParser)
        {
            _httpClient = httpClient;
            _cnbCsvParser = cnbCsvParser;
        }

        public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await _httpClient.GetAsync(SourceUrl);
            if (!response.IsSuccessStatusCode)
                return Result.Fail($"Retreival from {SourceUrl} failed with status code {response.StatusCode}. Response {await response.Content.ReadAsStringAsync()}");

            var stream = await response.Content.ReadAsStreamAsync();

            var result = _cnbCsvParser.TryParseExchangeRates(stream);

            return result switch
            {
                { IsFailed: true } => Result.Fail(result.Errors),
                { IsSuccess: true } => Result.Ok(result.Value
                    .Where(c => currencies.Any(x => x.Code == c.Code))
                    .Select(c => new ExchangeRate(c.Code, DefaultCurrencyCode, Math.Round(c.Rate / c.Amount, 8))))
            };
        }
    }
}
