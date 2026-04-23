namespace ExchangeRateUpdater.Domain.Exceptions;

public class FetchExchangeRatesFailException : Exception
{
	public FetchExchangeRatesFailException() : base("Failed to fetch exchange rates.")
	{
	}

	public FetchExchangeRatesFailException(string? message) : base(message)
	{
	}

	public FetchExchangeRatesFailException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}