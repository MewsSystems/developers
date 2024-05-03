using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Core
{
	/// <summary>
	/// This interface defines a query
	/// </summary>
	/// <typeparam name="TRequest">The type of the request model for this query</typeparam>
	/// <typeparam name="TResponse">The type of the reponse model for this query</typeparam>
	public interface IQuery<TRequest, TResponse>
	{
		/// <summary>
		/// This method is called to execute this query.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<TResponse> ExecuteAsync(TRequest request);
	}
}
