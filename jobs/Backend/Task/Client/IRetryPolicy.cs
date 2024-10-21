using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
	public interface IRetryPolicy
	{
		Task<HttpResponseMessage> ExecuteGetRequestWithRetry(HttpClient client, string relativeUri, int maxRetries, int requestInterval);
	}
}
