namespace ExchangeRate.Api.Validators
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// 
    /// </summary>
    public class NotNullAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return value == null
                ? new ValidationResult($"{validationContext.DisplayName} is required.")
                : ValidationResult.Success;
        }
    }
}
