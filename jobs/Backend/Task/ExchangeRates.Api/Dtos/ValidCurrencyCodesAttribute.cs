using System.ComponentModel.DataAnnotations;

namespace ExchangeRates.Api.Dtos
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ValidCurrencyCodesAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable<string> arr)
            {
                foreach (var c in arr)
                {
                    if (string.IsNullOrWhiteSpace(c) || !System.Text.RegularExpressions.Regex.IsMatch(c, "^[A-Za-z]{3}$"))
                        return new ValidationResult("All currency codes must be exactly 3 alphabetic characters.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
