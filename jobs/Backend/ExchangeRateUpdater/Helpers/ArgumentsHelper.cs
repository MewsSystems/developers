using System;

namespace ExchangeRateUpdater.Helpers;

public static class ArgumentsHelper
{
    /// <summary>
    /// The method checks that the value is not null.
    /// If false, ArgumentNullException will be thrown.
    /// </summary>
    /// <param name="name">The name of parameter.</param>
    /// <param name="value">The value of parameter.</param>
    public static void ThrowIfNull(string name, object value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(name);
        }
    }
}