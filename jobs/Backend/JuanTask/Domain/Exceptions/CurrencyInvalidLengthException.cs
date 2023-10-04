namespace Domain.Exceptions
{
    public class CurrencyInvalidLengthException: Exception
    {

        public CurrencyInvalidLengthException(string currency)
            : base($"Currency length must be 3, input: {currency}")
        { 

        }

    }
}
