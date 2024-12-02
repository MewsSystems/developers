using Mews.Caching.Common;

namespace Mews.Caching
{
    public interface IWriteOnlyCache
    {
        void Set<T>(string keyName, T value) where T : class;

        void Set<T>(string keyName, T value, CancellationToken cancellationToken) where T : class;

        void Set<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow) where T : class;

        void Set<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken) where T : class;

        Task SetAsync<T>(string keyName, T value) where T : class;

        Task SetAsync<T>(string keyName, T value, CancellationToken cancellationToken) where T : class;

        Task SetAsync<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow) where T : class;

        Task SetAsync<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken) where T : class;

    }
}
