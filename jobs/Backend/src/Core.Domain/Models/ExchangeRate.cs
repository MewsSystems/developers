using CSharpFunctionalExtensions;

namespace Core.Domain.Models
{
    public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
    {
        public static Result<ExchangeRate> Create(Currency sourceCurrency, Currency targetCurrency, string value, string baseAmount)
        {
            if (sourceCurrency == null)
            {
                return Result.Failure<ExchangeRate>($"Missing information: {nameof(sourceCurrency)}.");
            }

            if (targetCurrency == null)
            {
                return Result.Failure<ExchangeRate>($"Missing information: {nameof(targetCurrency)}.");
            }

            if (!decimal.TryParse(value, out decimal parsedValue) || parsedValue < 0)
            {
                return Result.Failure<ExchangeRate>($"Exchange rate is not valid.");
            }

            if (!int.TryParse(baseAmount, out int parsedBaseAmount) || parsedBaseAmount < 0)
            {
                return Result.Failure<ExchangeRate>($"Base amount is not valid.");
            }

            return Result.Success(new ExchangeRate(sourceCurrency, targetCurrency, parsedValue/parsedBaseAmount));
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
