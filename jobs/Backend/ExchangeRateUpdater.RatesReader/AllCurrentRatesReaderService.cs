using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.RatesReader
{
    internal class AllCurrentRatesReaderService : IAllCurrentRatesReaderService
    {
        private readonly HttpClient _httpClient;
        //this can be injected from the service that will call htis httpClient factory, and have it store in the appsettings/env variables
        //this way whenever the link changes there will be a change on env variables in the deployment and not a code change
        private const string CNBDailyRateExchange = @"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"

        public AllCurrentRatesReaderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Result<IEnumerable<CurrencyExchangeRate>>> GetAllExchangeRates()
        {
            var exchangeRatesReadModel = CurrencyFileParser.ParseFileToExchangeRatesReadModel(await _httpClient.GetStringAsync(CNBDailyRateExchange));
            var exchangeRateResults = exchangeRatesReadModel.Select(model => ExchangeRateFactory.CreateExchangeRateFromCZK(model));
            var combinedResult = Result.Combine(exchangeRateResults.ToArray());
            
            // Here we are flattening the results from all validation on each currency rate so it will make it easy upstream to expose this errors
            return combinedResult.Succsess ? 
                Result.OK(exchangeRateResults.Select(r => r.Value)) : 
                Result.Fail<IEnumerable<CurrencyExchangeRate>>(combinedResult.FailureResons);
        }
    }
}
