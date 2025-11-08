namespace ExchangeRateProviders.Core.Exception
{
	[Serializable]
	public class InvalidCurrencyException : ArgumentException
	{
		public IReadOnlyList<string> InvalidCurrencyCodes { get; }

		public InvalidCurrencyException(IEnumerable<string> invalidCodes)
			: base(BuildMessage(invalidCodes))
		{
			InvalidCurrencyCodes = invalidCodes.ToList().AsReadOnly();
		}

		public InvalidCurrencyException(IEnumerable<string> invalidCodes, ArgumentException inner)
			: base(BuildMessage(invalidCodes), inner)
		{
			InvalidCurrencyCodes = invalidCodes.ToList().AsReadOnly();
		}

		private static string BuildMessage(IEnumerable<string> invalidCodes)
			=> $"Invalid currency codes: {string.Join(", ", invalidCodes)}";
	}
}
