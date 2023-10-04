namespace Domain.Exceptions
{
    public class ExchangeRateAmountMustBeGreaterThanZeroException: Exception
    {

        public ExchangeRateAmountMustBeGreaterThanZeroException(decimal value)
            : base($"Value must be greater than 0, input: {value}")
        {

        }

    }
}
