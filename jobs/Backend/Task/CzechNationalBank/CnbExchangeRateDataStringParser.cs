using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.CzechNationalBank
{
	internal class CnbExchangeRateDataStringParser : IDataStringParser<IEnumerable<ExchangeRate>>
	{
		private const string targetCurrencyCode = "CZK";

		public IEnumerable<ExchangeRate> Parse(string input)
		{
			var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

			return lines
				.Skip(2)
				.Select(ParseExchangeRateLine)
				.Where(line => line != null);
		}

		private static ExchangeRate ParseExchangeRateLine(string line)
		{
			var cells = line.Split('|');
			var rawSourceAmount = cells[2];
			var rawCurrencyCode = cells[3];
			var rawTargetAmount = cells[4];

			if (decimal.TryParse(rawSourceAmount, out var sourceAmount) && decimal.TryParse(rawTargetAmount, NumberStyles.Currency, CultureInfo.InvariantCulture, out var targetAmount))
			{
				var rate = targetAmount / sourceAmount;

				return new ExchangeRate(new Currency(rawCurrencyCode), new Currency(targetCurrencyCode), rate);
			}

			return null;
		}
	}
}
