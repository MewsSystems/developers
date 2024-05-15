namespace ExchangeRateUpdater.API.Models.RequestModels
{
    public class GetExchangeRatesRequest
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<string> TargetCurrencies { get; set; }
        public decimal RoundingDecimal { get; set; } // TODO: set default to 2
    }
}
