using System.Diagnostics;

namespace ExchangeRateUpdater.Domain.Extensions;

[DebuggerStepThrough]
public static class AsReadonlyExtensions
{
    public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> self)
    {
        return self switch
        {
            IReadOnlyList<T> list => list,
            _ => self.ToList()
        };
    }

    public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> self)
    {
        return self switch
        {
            IReadOnlyCollection<T> collection => collection,
            _ => self.ToList()
        };
    }
}
