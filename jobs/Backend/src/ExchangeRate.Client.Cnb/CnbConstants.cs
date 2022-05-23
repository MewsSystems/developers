using System.Globalization;

namespace ExchangeRate.Client.Cnb
{
	public static class CnbConstants
	{
		/// <summary>
		/// Api types
		/// </summary>
		public enum ApiType
		{
			CnbTxt,
			CnbXml
		}

		/// <summary>
		/// CNB api base currency
		/// </summary>
		public const string BaseCurrency = "CZK";

		/// <summary>
		/// CNB api date format
		/// </summary>
		public const string DateFormat = "dd.MM.yyyy";

		/// <summary>
		/// CNB api decimal separator format
		/// </summary>
		public static readonly NumberFormatInfo RateFormat = new() { NumberDecimalSeparator = "," };

		/// <summary>
		/// CNB text api header
		/// </summary>
		public static class TextPosition
		{
			public const int TxtPositionCountry = 0;
			public const int TxtPositionCurrency = 1;
			public const int TxtPositionAmount = 2;
			public const int TxtPositionCode = 3;
			public const int TxtPositionRate = 4;
		}

		/// <summary>
		/// Caching keys for CNB client
		/// </summary>
		public static class CacheKeys
		{
			public const string CacheKeyCnb = "ExchangeRateService_Cnb";
		}

	}
}
