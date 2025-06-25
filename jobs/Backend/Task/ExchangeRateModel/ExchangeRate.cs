namespace ExchangeRateModel
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, DateTime date)
            : this(sourceCurrency, targetCurrency, 0, date)
        {
        }
        
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime date)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            Date = date;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }
        
        public DateTime Date { get; }

        public string ExchangeRateName()
            => $"{SourceCurrency}/{TargetCurrency}";
        
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        public override bool Equals(object? obj)
        {
            if(obj == null || obj.GetType() != typeof(ExchangeRate))
                return false;
            var otherRate = (ExchangeRate)obj;
            return SourceCurrency.Equals(otherRate.SourceCurrency) &&
                   TargetCurrency.Equals(otherRate.TargetCurrency) &&
                   Value == otherRate.Value &&
                   Date.Year == otherRate.Date.Year &&
                   Date.Month == otherRate.Date.Month &&
                   Date.Day == otherRate.Date.Day;
        }
    }
}
