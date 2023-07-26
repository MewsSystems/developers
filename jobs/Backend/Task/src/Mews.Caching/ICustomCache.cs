using Mews.Caching.Common;

namespace Mews.Caching
{
    public interface ICustomCache : IReadOnlyCache, IWriteOnlyCache
    {
        Maybe<T> GetOrAdd<T>(string keyName, Func<T> source) where T : class;
        Maybe<T> GetOrAdd<T>(string keyName, Func<T> source, CancellationToken cancellationToken) where T : class;

        Task<Maybe<T>> GetOrAddAsync<T>(string keyName, Func<Task<T>> source) where T : class;
        Task<Maybe<T>> GetOrAddAsync<T>(string keyName, Func<Task<T>> source, CancellationToken cancellationToken) where T : class;


    }
}
