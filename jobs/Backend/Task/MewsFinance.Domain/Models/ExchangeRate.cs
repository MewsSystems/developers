namespace MewsFinance.Domain.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, int currencyAmount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            CurrencyAmount = currencyAmount;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public int CurrencyAmount { get; }

        public bool IsCurrencyUnitAmount => CurrencyAmount == 1;
    }
}
