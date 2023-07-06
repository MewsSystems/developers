using CnbServiceClient.DTOs;

namespace CnbServiceClient.Interfaces
{
	public interface IExratesService
	{
		/// <summary>
		/// Gets the daily Exrates.
		/// </summary>
		/// <returns>IEnuremable object with all the <see cref="Exrate"/> available.</returns>
		Task<IEnumerable<Exrate>> GetExratesDailyAsync();
	}
}

