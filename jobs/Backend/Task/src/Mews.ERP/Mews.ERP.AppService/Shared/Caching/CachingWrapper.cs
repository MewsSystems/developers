using System.Diagnostics.CodeAnalysis;
using Mews.ERP.AppService.Shared.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Mews.ERP.AppService.Shared.Caching;

/// <summary>
/// Wrapper class to contain caching methods. Didn't have time to add unit tests.
/// </summary>
[ExcludeFromCodeCoverage]
public class CachingWrapper : ICachingWrapper
{
    private readonly IMemoryCache memoryCache;

    public CachingWrapper(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public TItem Set<TItem>(object key, TItem value, DateTimeOffset expirationRelativeToNow) 
        => memoryCache.Set(key, value, expirationRelativeToNow);

    public bool TryGetValue<TItem>(object key, out TItem? value) => memoryCache.TryGetValue(key, out value);
}