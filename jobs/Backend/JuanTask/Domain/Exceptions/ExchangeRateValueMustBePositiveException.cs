namespace Domain.Exceptions
{
    public class ExchangeRateValueMustBePositiveException: Exception
    {

        public ExchangeRateValueMustBePositiveException(decimal value)
            : base($"Value must be positive, input: {value}")
        {

        }

    }
}
