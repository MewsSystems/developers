using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.WebApi.Models
{
    public class Currency
    {
        public const string CzechCurrencyCode = "CZK";

        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        [Display(Name = "Currency code")]
        [Required(ErrorMessage = "{0} must be supplied")]
        [RegularExpression("^[A-Z]*$", ErrorMessage = "{0} should contain only uppercase letters")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "{0} should contain exactly 3 letters")]
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
