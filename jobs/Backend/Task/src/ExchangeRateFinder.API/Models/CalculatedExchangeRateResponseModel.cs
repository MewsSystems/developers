namespace ExchangeRateFinder.API.ViewModels
{
    public class CalculatedExchangeRateResponseModel
    {
        public string SourceCurrencyCode { get; init; } = string.Empty;

        public string TargetCurrencyCode { get; init; } = string.Empty;

        public decimal Rate { get; init; }
    }
}
