namespace ExchangeRateUpdater.API.Models.ResponseModels
{
    public class GetExchangeRatesResponse
    {
        public string TargetCurrency { get; set; }
        public string Date { get; set; }
        public IEnumerable<string> ExchangeRates { get; set; }
    }
}
