using ExchangeRate.Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.Api.Models
{
    public class CurrencyModel
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        [NotNull, DataType(DataType.Text), Display(Name = "Currency code")]        
        [RegularExpression("^[A-Z]*$", ErrorMessage = "Currency code must be uppercase.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be 3 characters long.")]
        public string Code { get; set; } = string.Empty;
    }
}