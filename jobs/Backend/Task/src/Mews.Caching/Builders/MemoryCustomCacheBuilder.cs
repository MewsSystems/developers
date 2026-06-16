using Mews.Caching.InMemory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Mews.Caching.Builders
{
    public class MemoryCustomCacheBuilder : ICustomCacheBuilder
    {
        private readonly ILogger<ICustomCache> _logger;
        private readonly CustomCacheOptions _options;
        private readonly IMemoryCache _memoryCache;

        public MemoryCustomCacheBuilder(ILogger<ICustomCache> logger, IMemoryCache memoryCache, CustomCacheOptions options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public ICustomCache Build()
        {
            return new InMemoryCustomCache(_memoryCache, _options, _logger);
        }
    }
}
