using System;
using System.Runtime.CompilerServices;

namespace ExchangeRateUpdater.Extensions;

public static class Strings
{
    public static string NotNullOrEmpty(this string str, [CallerArgumentExpression("str")] string paramName = null)
    {
        if (str is null)
        {
            throw new ArgumentNullException(paramName);
        }
        if (str.Length is 0)
        {
            throw new ArgumentException($"{paramName} is empty");
        }
        return str;
    }
}