using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExchangeRate.Domain
{
	public class Currency
	{
		public Currency(string? code)
		{
			if (string.IsNullOrEmpty(code))
			{
				throw new ArgumentNullException(code, "Currency code cannot be empty");
			}

			if (code.Length != 3 || !Regex.IsMatch(code, @"^[A-Z]+$"))
			{
				throw new ValidationException($"Currency code {code} should have 3 characters. Check ISO 4217");
			}

			Code = code;
		}

		/// <summary>
		///     Three-letter ISO 4217 code of the currency.
		/// </summary>
		[Required]
		[Range(3, 3, ErrorMessage = "The currency should have 3 characters => check ISO 4217.")]
		[Display(Name = "Currency code (three digits)")]
		public string Code { get; }

		public override string ToString() => Code;
	}
}
