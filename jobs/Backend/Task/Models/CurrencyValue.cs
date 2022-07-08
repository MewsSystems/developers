namespace ExchangeRateUpdater.Models
{
    public class CurrencyValue
    {
        public CurrencyValue(string code, decimal value)
        {
            Code = code;
            Value = value;
        }

        public string Code { get; }
        public decimal Value { get; }
    }
}