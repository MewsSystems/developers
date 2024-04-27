namespace ExchangeRateFinder.ConsoleApp.Responses.Models
{
    public class CalculatedExchangeRate
    {
        public string SourceCurrencyCode { get; set; }

        public string TargetCurrencyCode { get; set; }

        public decimal Rate { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrencyCode}/{TargetCurrencyCode}={Rate:F3}";
        }
    }
}
