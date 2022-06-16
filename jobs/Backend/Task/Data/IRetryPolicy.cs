namespace ExchangeRateUpdater.Data
{
    using ExchangeRateUpdater.Domain;
    using System;

    public interface IRetryPolicy<T>
    {
        /// <summary>
        /// Execute action with a retry policy
        /// </summary>
        /// <param name="action">The action to apply a retry policy to</param>
        /// <returns>Returns Bank Details</returns>
        T ExecuteWithRetry(Func<T> action);
    }
}
