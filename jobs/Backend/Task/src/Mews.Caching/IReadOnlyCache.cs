
using Mews.Caching.Common;

namespace Mews.Caching
{
    public interface IReadOnlyCache
    {
        Maybe<T> Get<T>(string keyname) where T : class;

        Maybe<T> Get<T>(string keyname, CancellationToken cancellationToken) where T : class;


        Task<Maybe<T>> GetAsync<T>(string keyname) where T : class;
        Task<Maybe<T>> GetAsync<T>(string keyname, CancellationToken cancellationToken) where T : class;

    }
}
