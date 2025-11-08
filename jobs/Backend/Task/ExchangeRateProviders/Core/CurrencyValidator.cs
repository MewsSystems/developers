using ExchangeRateProviders.Core.Exception;
using ExchangeRateProviders.Core.Model;

namespace ExchangeRateProviders.Core
{
	public static class CurrencyValidator
	{
		public static void ValidateCurrencyCodes(IEnumerable<Currency> currencies)
		{
			if (currencies == null)
			{
				return;
			}

			var invalidCurrencies = currencies
				.Where(c => !IsValidCurrencyCode(c.Code))
				.Select(c => c.Code ?? "null")
				.ToList();

			if (invalidCurrencies.Any())
			{
				throw new InvalidCurrencyException(invalidCurrencies);
			}
		}

		private static bool IsValidCurrencyCode(string? code)
		{
			if (string.IsNullOrWhiteSpace(code) ||
				code.Length != 3 ||
				!code.All(char.IsLetter))
			{
				return false;
			}

			return _iso4217Currencies.Contains(code);
		}

		private static readonly HashSet<string> _iso4217Currencies = new(StringComparer.OrdinalIgnoreCase)
		{
			// Major currencies
			"USD", "EUR", "GBP", "JPY", "CHF", "CAD", "AUD", "NZD",

			// Asia & Pacific
			"CNY", "INR", "KRW", "SGD", "HKD", "TWD", "THB", "MYR", "IDR", "PHP", "VND",
			"PKR", "BDT", "LKR", "NPR", "MMK", "KHR", "LAK", "MNT", "PGK", "FJD", "WST",

			// Europe
			"SEK", "NOK", "DKK", "PLN", "CZK", "HUF", "BGN", "RON", "HRK", "RUB", "TRY",
			"ISK", "UAH", "BYN", "MKD", "MDL", "GEL", "AMD", "AZN", "ALL", "BAM", "RSD",

			// Middle East
			"ILS", "AED", "SAR", "KWD", "QAR", "OMR", "BHD", "JOD", "LBP", "SYP", "YER",
			"IQD", "IRR", "AFN", "PKR",

			// Africa
			"EGP", "ZAR", "NGN", "KES", "TZS", "UGX", "GHS", "MAD", "TND", "DZD", "LYD",
			"SDG", "ETB", "MZN", "AOA", "NAD", "BWP", "LSL", "SZL", "MWK", "ZMW", "SOS",
			"RWF", "BIH", "SLL", "CVE", "XOF", "XAF", "KMF",

			// Americas
			"MXN", "BRL", "ARS", "CLP", "COP", "PEN", "UYU", "PYG", "BOB", "VEF", "VES",
			"GYD", "SRD", "BBD", "BSD", "TTD", "JMD", "HTG", "DOP", "CUP", "KYD", "BZD",

			// Oceania microstates
			"TVD", "KID", "TOP", "VUV"
		};
	}
}
