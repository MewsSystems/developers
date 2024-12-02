using System;

namespace ExchangeRateUpdater
{
    public static class EnvironmentHelper
    {
        public static string GetEnvironmentVariable(string variable)
        {
            string value = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Environment variable '{variable}' is not set.");
            }
            return value;
        }
    }
}
