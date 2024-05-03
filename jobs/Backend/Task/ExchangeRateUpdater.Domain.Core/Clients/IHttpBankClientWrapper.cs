using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Core.Clients
{
	public interface IHttpBankClientWrapper
	{
		Task<HttpResponseMessage> GetAsync(string requestUri);
	}
}
