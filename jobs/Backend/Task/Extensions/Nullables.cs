using System;
using System.Runtime.CompilerServices;

namespace ExchangeRateUpdater.Extensions;

public static class Nullables
{
    public static T NotNull<T>(this T value, [CallerArgumentExpression("value")] string paramName = null)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
        return value;
    }
}