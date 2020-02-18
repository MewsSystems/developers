namespace ExchangeRateUpdater
{
    public class FxRateCnb
    {
        public string SourceCurrency => "CZK";
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public int Amount { get; set; }
        public decimal Value => Rate / Amount;
    }
}
