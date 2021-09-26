using System;

namespace ExchangeRateUpdater.Utilities.Extensions
{
    public static class Guard
    {
        public static void ArgumentNotNull<T>(string argName, T argValue) where T : class
        {
            if (argValue == null)
            {
                throw new ArgumentNullException($"Argument cannot be null '{argName}'");
            }
        }

        public static void ArgumentStringNotEmpty(string argName, string argValue)
        {
            ArgumentNotNull(argName, argValue);

            if (argValue.Length == 0)
            {
                throw new ArgumentNullException($"Argument cannot be empty '{argName}'");
            }
        }
    }
}
