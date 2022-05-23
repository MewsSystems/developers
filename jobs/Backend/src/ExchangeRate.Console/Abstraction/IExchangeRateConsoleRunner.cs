using ExchangeRate.Console.Models.Enums;

namespace ExchangeRate.Console.Abstraction
{
	/// <summary>
	/// Interface for executing Get exchange rate
	/// </summary>
	public interface IExchangeRateConsoleRunner
	{
		/// <summary>
		/// Execute get exchange rate
		/// </summary>
		/// <param name="args">Arguments</param>
		/// <returns>Exit codes</returns>
		Task<ExitCode> ExecuteGetExchangeRates(string[] args);
	}
}
