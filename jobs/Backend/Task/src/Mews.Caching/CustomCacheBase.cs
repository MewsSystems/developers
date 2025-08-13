using Mews.Caching.Common;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Mews.Caching
{

    public abstract class CustomCacheBase : ICustomCache
    {
        protected readonly CustomCacheOptions _options;

        protected readonly ILogger<ICustomCache> _logger;

        protected CustomCacheBase(CustomCacheOptions options, ILogger<ICustomCache> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Maybe<T> Get<T>(string keyName) where T : class
            => Get<T>(keyName, CancellationToken.None);

        public Maybe<T> Get<T>(string keyName, CancellationToken cancellationToken) where T : class
            => GetAsync<T>(keyName, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

        public Task<Maybe<T>> GetAsync<T>(string keyName) where T : class
            => GetAsync<T>(keyName, CancellationToken.None);

        public abstract Task<Maybe<T>> GetAsync<T>(string keyName, CancellationToken cancellationToken) where T : class;

        public Maybe<T> GetOrAdd<T>(string keyName, Func<T> source) where T : class
            => GetOrAdd(keyName, source, _options.DefaultAbsoluteExpirationRelativeToNow);

        public Maybe<T> GetOrAdd<T>(string keyName, Func<T> source, CancellationToken cancellationToken) where T : class
            => GetOrAdd(keyName, source, _options.DefaultAbsoluteExpirationRelativeToNow, cancellationToken);

        public Maybe<T> GetOrAdd<T>(string keyName, Func<T> source, TimeSpan? absoluteExpirationRelativeToNow) where T : class
            => GetOrAdd(keyName, source, absoluteExpirationRelativeToNow, CancellationToken.None);

        public Maybe<T> GetOrAdd<T>(string keyName, Func<T> source, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = Get<T>(keyName, cancellationToken);
            if (!result.HasValue)
            {
                result = source.Invoke();
                Set(keyName, result.Value, absoluteExpirationRelativeToNow, cancellationToken);
            }

            return result;
        }

        public Task<Maybe<T>> GetOrAddAsync<T>(string keyName, Func<Task<T>> source) where T : class
            => GetOrAddAsync(keyName, source, _options.DefaultAbsoluteExpirationRelativeToNow, CancellationToken.None);

        public Task<Maybe<T>> GetOrAddAsync<T>(string keyName, Func<Task<T>> source, CancellationToken cancellationToken) where T : class
            => GetOrAddAsync(keyName, source, _options.DefaultAbsoluteExpirationRelativeToNow, cancellationToken);

        public Task<Maybe<T>> GetOrAddAsync<T>(string keyName, Func<Task<T>> source, TimeSpan? absoluteExpirationRelativeToNow) where T : class
            => GetOrAddAsync(keyName, source, absoluteExpirationRelativeToNow, CancellationToken.None);

        public async Task<Maybe<T>> GetOrAddAsync<T>(string keyName, Func<Task<T>> source, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await GetAsync<T>(keyName, cancellationToken);
            if (!result.HasValue)
            {
                result = await source.Invoke();
                await SetAsync(keyName, result.Value, absoluteExpirationRelativeToNow, cancellationToken);
            }

            return result;
        }

        public void Set<T>(string keyName, T value) where T : class
            => Set(keyName, value, CancellationToken.None);

        public void Set<T>(string keyName, T value, CancellationToken cancellationToken) where T : class
            => SetAsync(keyName, value, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

        public void Set<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow) where T : class
            => Set(keyName, value, absoluteExpirationRelativeToNow, CancellationToken.None);

        public void Set<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken) where T : class
            => SetAsync(keyName, value, absoluteExpirationRelativeToNow, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

        public Task SetAsync<T>(string keyName, T value) where T : class
            => SetAsync(keyName, value, _options.DefaultAbsoluteExpirationRelativeToNow);

        public Task SetAsync<T>(string keyName, T value, CancellationToken cancellationToken) where T : class
            => SetAsync(keyName, value, _options.DefaultAbsoluteExpirationRelativeToNow, cancellationToken);

        public Task SetAsync<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow) where T : class
            => SetAsync(keyName, value, absoluteExpirationRelativeToNow, CancellationToken.None);

        public abstract Task SetAsync<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken) where T : class;

        protected virtual T DeserializeValue<T>(string cacheValue, CancellationToken cancellationToken) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            return JsonSerializer.Deserialize<T>(cacheValue);
        }

        protected virtual string SerializeValue<T>(T value, CancellationToken cancellationToken) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            return JsonSerializer.Serialize(value);
        }

    }

}