namespace Adapter.ExchangeRateProvider.InMemory;

/// <summary>
/// Type of caches.
/// All - cache for all fx rates.
/// Selected - cache for one source currency fx rates.
/// </summary>
internal enum CacheType
{
    All = 1,
    Selected = 2
}
