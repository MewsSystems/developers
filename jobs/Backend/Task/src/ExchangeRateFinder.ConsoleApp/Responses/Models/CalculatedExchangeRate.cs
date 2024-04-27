namespace ExchangeRateFinder.ConsoleApp.Responses.Models
{
    public class CalculatedExchangeRate
    {
        public string SourceCurrency { get; set; }

        public string TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value:F3}";
        }
    }
}
