namespace ERU.Application.Exceptions;

public class InvalidConfigurationException : Exception
{
	public InvalidConfigurationException(string message) : base(
		$"Invalid use of configuration. The property {message} is not set.")
	{
	}
}