using Common.Exceptions;
using Common.Logs;

namespace Common.Model
{
	/// <summary>
	/// Class which holds information about a currency.
	/// </summary>
	public class Currency
	{
		#region Properties

		/// <summary>
		/// Three-letter ISO 4217 code of the currency.
		/// </summary>
		public string Code { get; }

		#endregion

		#region Constructor

		public Currency(string code)
		{
			if (code.Length == 3)
				Code = code;
			else
			{
				Log.Instance.Error($"Currency code is in incorrect format. Code {code}");
				throw new IncorrectCurrencyCodeFormartException();
			}
				
		}

		#endregion

		#region Public methods.

		public override string ToString()
		{
			return Code;
		}

		#endregion
	}
}
