using JetBrains.Annotations;

namespace ExchangeRateUpdater.Lib.Collection;

public static class EnumerableExtensions
{
    [ContractAnnotation("null=>true")]
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable) => enumerable == null || !enumerable.Any();
}