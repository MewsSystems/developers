namespace ExchangeRateFinder.API.ViewModels
{
    public class CalculatedExchangeRateResponseModel
    {
        public string SourceCurrencyCode { get; set; }

        public string TargetCurrencyCode { get; set; }

        public decimal Rate { get; set; }
    }
}
