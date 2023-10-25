using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Core.Models;

public class Currency
{
	public Currency(string code)
	{
		Code = code;
	}

	/// <summary>
	///     Three-letter ISO 4217 code of the currency.
	/// </summary>
	public string Code { get; }

	public override string ToString()
	{
		return Code;
	}
}

public class CurrencyComparer : IEqualityComparer<Currency>
{
	public bool Equals(Currency? x, Currency? y)
	{
		if (x is null && y is null)
		{
			return true;
		}

		return x!.Code.Equals(y!.Code, StringComparison.InvariantCulture);
	}

	public int GetHashCode(Currency obj) => obj.Code.GetHashCode();
}