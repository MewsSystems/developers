using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace ExchangeRate.Api.Validators
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateDateFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            string dateStr = value.ToString();
            if (!DateTime.TryParseExact(dateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return new ValidationResult($"The {validationContext.DisplayName} must be in the format dd-MM-yyyy.");
            }

            return ValidationResult.Success;
        }
    }

}
