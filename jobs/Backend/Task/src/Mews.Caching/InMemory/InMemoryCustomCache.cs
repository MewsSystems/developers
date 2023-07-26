using Mews.Caching.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Mews.Caching.InMemory
{
    public class InMemoryCustomCache : CustomCacheBase
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCustomCache(
           IMemoryCache memoryCache,
           CustomCacheOptions options,
           ILogger<ICustomCache> logger) : base(options, logger)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public override Task<Maybe<T>> GetAsync<T>(string keyName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Maybe<T> result = new Maybe<T>(default);

            try
            {
                var cacheValue = _memoryCache.Get<string>(keyName);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    result = DeserializeValue<T>(cacheValue, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: Cannot get {keyName} from the custom cache {name}", keyName, _options.Name);
                throw;
            }


            return Task.FromResult(result);
        }

        public override Task SetAsync<T>(string keyName, T value, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                string serializedValue = SerializeValue(value, cancellationToken);
                if (absoluteExpirationRelativeToNow.HasValue)
                {
                    _memoryCache.Set(keyName, serializedValue, absoluteExpirationRelativeToNow.Value);
                }
                else
                {
                    _memoryCache.Set(keyName, serializedValue);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: Cannot add {keyName} to the custom cache {name}", keyName, _options.Name);
                throw;
            }


            return Task.CompletedTask;
        }
    }
}
