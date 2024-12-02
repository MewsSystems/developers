using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Model
{
    /// <param name="Code"> Three-letter ISO 4217 code of the currency. </param>
    public record Currency([StringLength(3, MinimumLength = 3)] string Code)
    {
        public override string ToString()
        {
            return Code;
        }
    }
}
