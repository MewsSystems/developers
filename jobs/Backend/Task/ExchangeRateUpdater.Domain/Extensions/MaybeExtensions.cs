using ExchangeRateUpdater.Domain.Common;

namespace ExchangeRateUpdater.Domain.Extensions;

public static class MaybeExtensions
{
    public static Maybe<T> AsMaybe<T>(this T? self) => self;
    public static Maybe<T> AsMaybe<T>(this T? self) where T : struct => self ?? Maybe<T>.Nothing;
    public static Maybe<T> AsMaybe<T>(this object self) where T : class => self as T;
}
