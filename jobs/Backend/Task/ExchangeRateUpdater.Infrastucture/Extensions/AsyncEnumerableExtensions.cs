using System.Runtime.CompilerServices;

namespace ExchangeRateUpdater.Infrastucture.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> source, Predicate<T> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var item in source.WithCancellation(cancellationToken))
            if (predicate(item))
                yield return item;
    }
}