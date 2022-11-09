namespace ERU.Application.Exceptions;

public class EmptyExchangeRateResponseException : Exception
{
	public EmptyExchangeRateResponseException(string codes) : base($" No exchange rates were found: Currencies {codes} have no rates.")
	{
	}
}