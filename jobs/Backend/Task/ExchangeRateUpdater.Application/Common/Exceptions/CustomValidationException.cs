namespace ExchangeRateUpdater.Application.Common.Exceptions;

public class CustomValidationException : Exception
{
    protected CustomValidationException(string message) : base(message) { }
    protected CustomValidationException(string message, Exception innerException) : base(message, innerException) { }
}