namespace Domain.Exceptions
{
    public class CurrencyMustBeAllLettersException: Exception
    {

        public CurrencyMustBeAllLettersException(string currency)
            : base($"All characters must be letters, input: {currency}")
        {

        }

    }
}
