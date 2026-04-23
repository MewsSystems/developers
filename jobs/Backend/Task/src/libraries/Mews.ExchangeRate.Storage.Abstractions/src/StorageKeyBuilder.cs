using Mews.ExchangeRate.Domain.Models;
using Mews.ExchangeRate.Storage.Abstractions.Models;

namespace Mews.ExchangeRate.Storage.Abstractions
{
    public static class StorageKeyBuilder
    {
        public const string StorageStateKey = nameof(StorageState);
        public const string ExchangeRateKeyPrefix = nameof(ExchangeRate);

        /// <summary>
        /// Builds the exchange rate key.
        /// </summary>
        /// <param name="sourceCurrency">The origin currency.</param>
        /// <param name="targetCurrency">The destination currency.</param>
        /// <returns></returns>
        public static string BuildExchangeRateKey(Currency sourceCurrency, Currency targetCurrency)
        {
            return $"{ExchangeRateKeyPrefix}:{sourceCurrency.Code}:{targetCurrency.Code}";
        }
    }
}