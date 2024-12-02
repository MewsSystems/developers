namespace ExchangeRateFinder.ConsoleApp.Responses.Models
{
    public class CalculatedExchangeRate
    {
        public string SourceCurrencyCode { get; init; }

        public string TargetCurrencyCode { get; init; }

        public decimal Rate { get; init; }

        public override string ToString()
        {
            return $"{SourceCurrencyCode}/{TargetCurrencyCode}={Rate:F3}";
        }
    }
}
