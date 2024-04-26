namespace ExchangeRateUpdater.Application
{
    public class ExchangeRateViewModel
    {
        public string SourceCurrency { get; set; }

        public string TargetCurrency { get; set; }

        public decimal Value { get; set; }
    }
}
