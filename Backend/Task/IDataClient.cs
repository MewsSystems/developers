using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public interface IDataClient
    {
		/// <summary>
		/// Gets data from the source.
		/// </summary>
		/// <returns>The content stream.</returns>
		/// <exception cref="DataClientException">Failed to obtain data.</exception>
		Task<Stream> GetDataAsync();
    }
}
