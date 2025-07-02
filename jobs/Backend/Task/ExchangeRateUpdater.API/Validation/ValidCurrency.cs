using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater.API.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ValidCurrencyAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null || value is not string)
			{
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName))
				{
					ErrorMessage = "400-1"
				};
			}

			bool result = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
				.Select(t =>
				{
					try
					{
						return new RegionInfo(t.Name);
					}
					catch
					{
						return null;
					}
				})
				.Select(c => c?.ISOCurrencySymbol)
				.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);

			if (result)
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName))
				{
					ErrorMessage = "400-2"
				};
			}
		}
	}
}
