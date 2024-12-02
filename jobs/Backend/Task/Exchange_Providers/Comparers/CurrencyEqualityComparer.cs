using ExchangeRateUpdater.Exchange_Providers.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Exchange_Providers.Comparers
{
    /// <summary>
    /// Provides a custom equality comparer for the Currency class based on the currency code.
    /// </summary>
    internal class CurrencyEqualityComparer : IEqualityComparer<Currency>
    {
        /// <summary>
        /// Determines whether two Currency objects are equal based on their currency codes.
        /// </summary>
        /// <param name="a">The first Currency object to compare.</param>
        /// <param name="b">The second Currency object to compare.</param>
        /// <returns>
        ///   <c>true</c> if the currency codes of the two Currency objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Currency a,  Currency b)
        {
            return a.Code == b.Code;
        }

        /// <summary>
        /// Returns a hash code for the specified Currency object based on its currency code.
        /// </summary>
        /// <param name="c">The Currency object for which to calculate the hash code.</param>
        /// <returns>A hash code for the Currency object based on its currency code.</returns>
        public int GetHashCode(Currency c)
        {
            return c.Code.GetHashCode();
        }
    }
}
