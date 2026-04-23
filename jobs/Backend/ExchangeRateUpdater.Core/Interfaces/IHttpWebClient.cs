using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Interfaces
{
    /// <summary>
    /// Provides an abstraction for making HTTP GET requests with built-in resilience policies.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface should:
    /// <list type="bullet">
    /// <item><description>Handle transient failures with automatic retries</description></item>
    /// <item><description>Maintain configured HTTP client settings (base address, headers, etc.)</description></item>
    /// <item><description>Validate responses before returning them</description></item>
    /// </list>
    /// </remarks>
    public interface IHttpWebClient
    {
        /// <summary>
        /// Executes a GET request to the configured endpoint.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// the <see cref="HttpResponseMessage"/> from the successful request.
        /// </returns>
        Task<HttpResponseMessage> GetAsync();
    }
}
