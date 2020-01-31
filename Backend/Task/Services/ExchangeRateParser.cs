using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater.Services
{
	public class ExchangeRateParser
	{
		private readonly Currency defaultCurrency;
		private readonly CultureInfo decimalFormatProvider;
		
		private readonly char lineSeparator;
		private readonly char valueSeparator;
		private readonly int skippedRows;

		private readonly int rateColumnIndex;
		private readonly int quantityColumnIndex;
		private readonly int targetCurrencyColumnIndex;
		private string expectedHeader;

		public ExchangeRateParser(string defaultCurrency, string decimalFormatProvider, char lineSeparator, char valueSeparator, int skippedRows,
			int rateColumnIndex, int quantityColumnIndex, int targetCurrencyColumnIndex, string expectedHeader)
		{
			this.defaultCurrency = new Currency(defaultCurrency);
			this.decimalFormatProvider = new CultureInfo(decimalFormatProvider);
			this.lineSeparator = lineSeparator;
			this.valueSeparator = valueSeparator;
			this.skippedRows = skippedRows;

			this.rateColumnIndex = rateColumnIndex;
			this.quantityColumnIndex = quantityColumnIndex;
			this.targetCurrencyColumnIndex = targetCurrencyColumnIndex;

			this.expectedHeader = expectedHeader;
		}


		public IEnumerable<ExchangeRate> Parse(string csv)
		{
			var lines = csv.Split(lineSeparator)
						   .Where(l => !string.IsNullOrWhiteSpace(l))
						   .ToList();

			CheckHeader(lines.Skip(skippedRows).First());

			return lines.Skip(skippedRows + 1)
						.Select(LineToExchangeRate);
		}

		private void CheckHeader(string header)
		{
			if(!header.Equals(expectedHeader, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new IncorrectCsvFormatException(expectedHeader, header);
			}
		}

		private string[] GetValues(string line)
		{
			return line.Split(valueSeparator);
		}

		private ExchangeRate LineToExchangeRate(string line)
		{
			var values = GetValues(line);

			var targetCurrency = new Currency(values[targetCurrencyColumnIndex]);
			var quantity = decimal.Parse(values[quantityColumnIndex], decimalFormatProvider);
			var rate = decimal.Parse(values[rateColumnIndex], decimalFormatProvider);

			return new ExchangeRate(defaultCurrency, targetCurrency, rate / quantity);
		}
	}
}
