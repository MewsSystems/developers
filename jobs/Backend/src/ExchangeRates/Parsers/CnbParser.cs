using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.Contracts;
using ExchangeRates.Providers;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Parsers
{
	public class CnbParser : ICnbParser
	{
		private const string rateRowsDelimiter = "\n";
		private const string rateColumnsDelimiter = "|";
		private const string sourceCurrencyCode = "CZK";
		private const byte numberOfHeaderRows = 2;
		private const byte numberOfRowColumns = 5;
		private const byte targetCurrencyUnitAmountIndex = 2;
		private const byte targetCurrencyCodeIndex = 3;
		private const byte exchangeRateValueIndex = 4;		
		private readonly ILogger<CnbParser> logger;
		private readonly ICnbCultureProvider culture;

		public CnbParser(
			ILogger<CnbParser> logger, 
			ICnbCultureProvider culture)
		{			
			this.logger = logger;
			this.culture = culture;
		}

		public ExchangeRate[] ParserData(string data)
		{			
			var rates = new List<ExchangeRate>();			
			var rateRows = data
				// Rate rows are delimited by rateRowsDelimiter, so it can be split. Empty entries are removed, since they have no meaning.
				.Split(rateRowsDelimiter, StringSplitOptions.RemoveEmptyEntries)
				// Rate columns are delimited by rateColumnsDelimiter, so it can be split.
				.Select(rateRow => rateRow.Split(rateColumnsDelimiter))
				.ToArray();
			
			// Skip the rate data processing if they are none or only header.
			if (rateRows.Length > numberOfHeaderRows) 
			{
				// Check the number of columns in data rows. Header rows are skipped.
				if (!rateRows.Skip(numberOfHeaderRows).Any(rateRow => rateRow.Length != numberOfRowColumns))
				{
					foreach (var rateRow in rateRows.Skip(numberOfHeaderRows))
					{
						rates.Add(
							new ExchangeRate(
								new Currency(sourceCurrencyCode),
								new Currency(rateRow[targetCurrencyCodeIndex]),
								Convert.ToInt16(rateRow[targetCurrencyUnitAmountIndex], culture.GetCultureInfo()),
								Convert.ToDecimal(rateRow[exchangeRateValueIndex], culture.GetCultureInfo())));						
					};
				}
				else
				{
					logger.LogWarning($"[{nameof(CnbParser)}] Number of columns in the exchange rate data set from CNB is not equal to the expected value, so it cannot be parsed.");
					return Array.Empty<ExchangeRate>();
				}

				logger.LogInformation($"[{nameof(CnbParser)}] Exchange rate data from CNB has been successfully parsed.");
				return rates.ToArray();
			}
			else 
			{
				logger.LogWarning($"[{nameof(CnbParser)}] Number of rows in the exchange rate data set from CNB is not equal to the expected value, so it cannot be parsed.");
			}

			return Array.Empty<ExchangeRate>();
		}		
	}
}
