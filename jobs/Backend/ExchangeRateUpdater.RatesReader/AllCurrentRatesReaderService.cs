using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.RatesReader
{
    public class AllCurrentRatesReaderService : IAllCurrentRatesReaderService
    {
        private readonly HttpClient _httpClient;

        public AllCurrentRatesReaderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Result<IEnumerable<CurrencyExchangeRate>>> GetAllExchangeRates()
        {
            //We we are planning to get any historical data here we can parameterize the URI
            var exchangeRatesReadModel = CurrencyFileParser.ParseFileToExchangeRatesReadModel(await _httpClient.GetStringAsync(_httpClient.BaseAddress));
            var exchangeRateResults = exchangeRatesReadModel.Select(model => ExchangeRateFactory.CreateExchangeRateFromCZK(model));
            var combinedResult = Result.Combine(exchangeRateResults.ToArray());
            
            // Here we are flattening the results from all validation on each currency rate so it will make it easy upstream to expose this errors
            return combinedResult.Succsess ? 
                Result.OK(exchangeRateResults.Select(r => r.Value)) : 
                Result.Fail<IEnumerable<CurrencyExchangeRate>>(combinedResult.FailureResons);
        }
    }
}
