namespace ExchangeRateFinder.ConsoleApp.Responses
{
    public class CalculatedExchangeRateResponse
    {
        public string SourceCurrency { get; set; }

        public string TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
