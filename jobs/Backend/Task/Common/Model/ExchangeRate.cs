namespace Common.Model
{
	/// <summary>
	/// Class which holds information about exchange rate.
	/// </summary>
	public class ExchangeRate
	{
		#region Properties

		public Currency SourceCurrency { get; }

		public Currency TargetCurrency { get; }

		public decimal Value { get; }

		public decimal MultipliedValue => Value / TargetMultiplicator;

		public int TargetMultiplicator { get; }

		public DateTime DateTime {get;}

		#endregion
		#region Constructor

		public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime dateTime) : this(sourceCurrency, targetCurrency, value, dateTime, 1) { }

		public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime dateTime, int targetMultiplicator)
		{
			SourceCurrency = sourceCurrency;
			TargetCurrency = targetCurrency;
			Value = value;
			TargetMultiplicator = targetMultiplicator;
			DateTime = dateTime;
		}

		#endregion

		#region Public methods.
		public override string ToString()
		{
			return $"{SourceCurrency}/{TargetCurrency}={MultipliedValue}";
		}

		#endregion
	}
}
