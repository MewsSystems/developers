using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Helpers;

public static class CollectionHelper
{
    /// <summary>
    /// The method checks that the collection is not null and contains elements.
    /// </summary>
    /// <param name="data">The list of items.</param>
    public static bool IsEmpty<T>(IEnumerable<T> data)
    {
        return data == null || !data.Any();
    }
}