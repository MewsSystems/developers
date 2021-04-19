using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public interface IHttpClientWrapper : IDisposable
	{
		Task<HttpResponseMessage> GetAsync(string url);
	}
}
