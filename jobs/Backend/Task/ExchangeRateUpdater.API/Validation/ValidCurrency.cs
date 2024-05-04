using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater.API.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ValidCurrencyAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null || value is not string)
			{
				return false;
			}

			return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
				.Select(t =>
				{
					try
					{
						return new RegionInfo(t.LCID);
					}
					catch
					{
						return null;
					}
				})
				.Select(c => c?.ISOCurrencySymbol)
				.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
		}
	}
}
