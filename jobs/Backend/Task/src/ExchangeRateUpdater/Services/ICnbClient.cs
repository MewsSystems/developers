namespace ExchangeRateUpdater.Services
{
	/// <summary>
	/// Gets FX rates from CNB.
	/// </summary>
	public interface ICnbClient
	{
		/// <summary>
		/// Gets FX rates text output from CNB.
		/// </summary>
		/// <param name="date">Date to get rates or <c>null</c> for the latest rate. This semantics reflects the CNB API.</param>
		/// <returns>FX rate text output from CNB.</returns>
		Task<string> GetRatesAsync(DateOnly? date = null);
	}
}