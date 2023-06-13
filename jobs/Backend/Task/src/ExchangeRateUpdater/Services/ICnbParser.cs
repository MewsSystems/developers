using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.Services
{
	/// <summary>
	/// Parses the CNB daily FX rates data.
	/// </summary>
	public interface ICnbParser
	{
		/// <summary>
		/// Parses the CNB daily FX rates data.
		/// </summary>
		/// <param name="data">Text output from CNB</param>
		/// <returns>Parsed daily FX rates data</returns>
		IEnumerable<ExchangeRate> Parse(string data);
	}
}