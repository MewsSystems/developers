using System.Net.Http;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Http.Abstractions
{
    /// <summary>
    /// Interface for HTTP client contract (useful for unit testing scenarios).
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}
