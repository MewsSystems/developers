using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRates.Clients
{
	public interface IClient<TOutputData>
	{
		Task<TOutputData> GetExchangeRatesAsync(DateOnly? date, CancellationToken token = default);
	}
}
