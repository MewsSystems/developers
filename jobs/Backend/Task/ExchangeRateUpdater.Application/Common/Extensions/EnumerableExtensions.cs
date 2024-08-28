namespace ExchangeRateUpdater.Application.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? input) => input?.Any() != true;
}