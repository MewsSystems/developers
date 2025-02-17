using System.ComponentModel.DataAnnotations;
namespace ExchangeRate.Api.Validators
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateCurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string currencyCode)
            {
                if (currencyCode.Length != 3)
                {
                    return new ValidationResult("Currency code must be exactly 3 characters long.");
                }
            }
            else
            {
                return new ValidationResult("Invalid currency code.");
            }

            return ValidationResult.Success;
        }
    }


}
