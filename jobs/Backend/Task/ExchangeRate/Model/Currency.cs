using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.ExchangeRate.Model
{
    /// <summary>
    /// Represents a currency using its three-letter ISO 4217 code.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Currency"/> class with the specified code.
    /// </remarks>
    /// <param name="code">The three-letter ISO 4217 code of the currency.</param>
    public class Currency(string code)
    {

        /// <summary>
        /// Gets the three-letter ISO 4217 code of the currency.
        /// </summary>
        [Required]
        public string Code { get; } = code;

        /// <summary>
        /// Returns the three-letter ISO 4217 code of the currency.
        /// </summary>
        /// <returns>The three-letter ISO 4217 code of the currency.</returns>
        public override string ToString()
        {
            return Code;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current currency.
        /// </summary>
        /// <param name="obj">The object to compare with the current currency.</param>
        /// <returns>true if the specified object is equal to the current currency; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }

            Currency otherCurrency = (Currency)obj;
            return Code == otherCurrency.Code;
        }

        /// <summary>
        /// Returns the hash code for the current currency.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
