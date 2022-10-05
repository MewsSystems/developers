using System.Globalization;

namespace ExchangeRates.Contracts
{
    public class ExchangeRate
    {
        public ExchangeRate(
            Currency sourceCurrency, 
            Currency targetCurrency, 
            short targetCurrencyUnitAmount, 
            decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            TargetCurrencyUnitAmount = targetCurrencyUnitAmount;
            Value = value;			
		}

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public short TargetCurrencyUnitAmount { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{Value} {SourceCurrency} => {TargetCurrencyUnitAmount} {TargetCurrency}";
        }
    }
}
