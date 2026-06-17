namespace Mews.ERP.AppService.Shared.Caching.Interfaces;

public interface ICachingWrapper
{
    TItem Set<TItem>(object key, TItem value, DateTimeOffset expirationRelativeToNow);

    bool TryGetValue<TItem>(object key, out TItem? value);
}