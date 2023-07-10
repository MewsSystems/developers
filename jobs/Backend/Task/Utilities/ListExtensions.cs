using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Utilities;

/// <summary>
/// Contains extension methods for handling lists and enumerables.
/// </summary>
internal static class ListExtensions
{
    /// <summary>
    /// Checks if the enumerable contains any elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="data">The enumerable to check.</param>
    /// <returns><c>true</c> if the enumerable is not null and contains any elements; otherwise, <c>false</c>.</returns>
    internal static bool IsAny<T>(this IEnumerable<T> data) =>
        data != null && data.Any();
}