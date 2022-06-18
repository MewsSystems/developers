using ExchangeRateUpdater;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdated.Service.Exceptions
{
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException(string message) : base(message)
        {

        }

        public static void ThrowIfInvalid(string currency)
        {
            if (currency.Length != 3)
                ThrowInvalidCurrencyException();
        }

        [DoesNotReturn]
        public static void ThrowInvalidCurrencyException()
        {
            throw new InvalidCurrencyException("Currency code can't be less than 3 characters");
        }
    }
}
