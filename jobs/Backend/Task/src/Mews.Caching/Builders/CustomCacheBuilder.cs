using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
namespace Mews.Caching.Builders
{
    public interface ICustomCacheBuilder
    {
        ICustomCache Build();
    }

    public class CustomCacheBuilder
    {
        private IMemoryCache _memoryCache;
        private readonly ILogger<ICustomCache> _logger;
        private ICustomCacheBuilder _builder;

        protected CustomCacheBuilder(ILogger<ICustomCache> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public CustomCacheBuilder Using(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            return this;
        }

        public CustomCacheBuilder Using(CustomCacheOptions options)
        {
            _builder = new MemoryCustomCacheBuilder(_logger, _memoryCache, options);

            return this;
        }

        public static CustomCacheBuilder Start(ILogger<ICustomCache> logger)
            => new CustomCacheBuilder(logger);

        public ICustomCache Build()
            => _builder.Build();
    }

}
