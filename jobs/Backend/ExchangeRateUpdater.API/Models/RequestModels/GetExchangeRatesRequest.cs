namespace ExchangeRateUpdater.API.Models.RequestModels
{
    public class GetExchangeRatesRequest
    {
        public IEnumerable<string> TargetCurrencies { get; set; }
        public decimal RoundingDecimal { get; set; } = 2;
    }
}
