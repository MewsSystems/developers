namespace Mews.CurrencyExchange.Domain.Exceptions
{
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException() : base() { }
        public InvalidCurrencyException(string? message) : base(message) { }
    }
}
