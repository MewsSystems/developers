
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Mews.Caching.InMemory;
using Mews.Caching.Builders;

namespace Mews.Caching
{
    public interface ICustomCacheFactory
    {
        ICustomCache GetOrCreate(string name);
    }

    public class CustomCacheFactory : ICustomCacheFactory
    {
        private readonly ConcurrentDictionary<string, ICustomCache> _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptionsMonitor<CustomCacheOptions> _optionsMonitor;
        private readonly ILogger<ICustomCache> _logger;

        public CustomCacheFactory(
            IMemoryCache memoryCache,
            IOptionsMonitor<CustomCacheOptions> optionsMonitor,
            ILogger<ICustomCache> logger)
        {
            _repository = new ConcurrentDictionary<string, ICustomCache>();
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ICustomCache GetOrCreate(string name)
        {
            if (!_repository.TryGetValue(name, out ICustomCache customCache))
            {
                customCache = CustomCacheBuilder.Start(_logger)
                                                .Using(_memoryCache)
                                                .Using(_optionsMonitor.Get(name))
                                                .Build();
                if (customCache != null)
                {
                    _repository.TryAdd(name, customCache);
                }
            }

            return customCache;
        }
    }



}
