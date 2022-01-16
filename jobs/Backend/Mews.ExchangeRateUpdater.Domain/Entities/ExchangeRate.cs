namespace Mews.ExchangeRateUpdater.Domain.Entities
{
    public class ExchangeRate
    {
        public string SourceCurrencyName { get; set; }

        public string TargetCurrencyName { get; set; }
        
        public int Amount { get; set; }
        
        public string Code { get; set; }
        
        public decimal Rate { get; set; }

        public override string ToString()
        {
            return $"1 {SourceCurrencyName} / {Amount} {TargetCurrencyName} = {Rate}";
        }
    }
}
