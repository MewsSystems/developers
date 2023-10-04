using Domain.Exceptions;

namespace Domain.Entities
{
    public class ExchangeRate
    {
        private ExchangeRate(Currency sourceCurrency, Currency targetCurrency, ExchangeRateValue value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public ExchangeRateValue Value { get; }

        public static ExchangeRate Create(string sourceCurrency,
                                          string targetCurrency,
                                          decimal value)
        {
            return new ExchangeRate(new Currency(sourceCurrency),
                                    new Currency(targetCurrency),
                                    new ExchangeRateValue(value));
        }

        public static ExchangeRate Create(string sourceCurrency,
                                          string targetCurrency,
                                          int amount,
                                          decimal value)
        {

            if (amount <= 0)
                throw new ExchangeRateAmountMustBeGreaterThanZeroException(amount);

            return new ExchangeRate(new Currency(sourceCurrency),
                                    new Currency(targetCurrency),
                                    new ExchangeRateValue(value / amount));
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value.Value}";
        }
    }
}
