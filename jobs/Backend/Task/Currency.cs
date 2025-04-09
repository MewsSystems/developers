namespace ExchangeRateUpdater
{
	public class Currency
	{
		public Currency(string code)
		{
			Code = code;
		}

		/// <summary>
		/// Three-letter ISO 4217 code of the currency.
		/// </summary>
		public string Code { get; }

		public override string ToString()
		{
			return Code;
		}

		// overload required to make IEnumerable::Contains() work
		public override bool Equals(object obj)
		{
			return obj is Currency other && this.Code == other.Code;
		}

		// overload required to make IEnumerable::Contains() work
		public override int GetHashCode()
		{
			return Code.GetHashCode();
		}
	}
}
