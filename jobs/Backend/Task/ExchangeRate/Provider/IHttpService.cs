/*
 * This interface represents an abstraction for an HTTP service that fetches data asynchronously.
 * Implementations of this interface can be injected into other components, facilitating unit testing and dependency inversion.
 */
using System.Threading.Tasks;
using System.Threading;

internal interface IHttpService
{
    /// <summary>
    /// Sends an asynchronous GET request to the specified URL and returns the response as a string.
    /// </summary>
    /// <param name="url">The URL to send the GET request to.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response content as a string.</returns>
    Task<string> GetAsync(string url, CancellationToken cancellationToken);
}
