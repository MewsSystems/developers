namespace ERU.Application.Exceptions;

public class EmptyExchangeRateResponseException : Exception
{
	public EmptyExchangeRateResponseException(string stockId) : base($"Stock with identifier: {stockId} has no price. Check the data resource.")
	{
	}
}