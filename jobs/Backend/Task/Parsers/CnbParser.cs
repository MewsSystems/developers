using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRates.Contracts;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Parsers
{
	public class CnbParser : ICnbParser
	{
		private const string rateRowsDelimiter = "\n";
		private const string rateColumnsDelimiter = "|";
		private const string domesticCurrencyCode = "CZK";
		private const byte numberOfHeaderRows = 2;
		private const byte numberOfRowColumns = 5;
		private const byte currencyCodeIndex = 3;
		private const byte exchangeRateValueIndex = 4;
		private readonly CultureInfo culture;
		private readonly ILogger<CnbParser> logger;

		public CnbParser(ILogger<CnbParser> logger)
		{
			// Create specific culture for exchange rate value conversion.
			culture = CultureInfo.CreateSpecificCulture("cs-CZ");
			this.logger = logger;
		}

		public ExchangeRate[] ParserData(string data)
		{			
			var rates = new List<ExchangeRate>();

			// Rate rows are delimited by rateRowsDelimiter, so it can be split. Empty entries are removed, since they have no meaning.
			var rateRows = data.Split(rateRowsDelimiter, StringSplitOptions.RemoveEmptyEntries);
			
			// Skip the rate data processing if they are none or only header.
			if (rateRows.Length > numberOfHeaderRows) 
			{
				foreach (var rateRow in rateRows.Skip(numberOfHeaderRows)) 
				{
					// Rate columns are delimited by rateColumnsDelimiter, so it can be split.
					var rateRowColumns = rateRow.Split(rateColumnsDelimiter);

					// Check the expected number of columns.
					if (rateRowColumns.Length == numberOfRowColumns) 
					{
						rates.Add(
							new ExchangeRate(								
								new Currency(rateRowColumns[currencyCodeIndex]),
								new Currency(domesticCurrencyCode),
								Convert.ToDecimal(rateRowColumns[exchangeRateValueIndex], culture)));
					}
					else
					{
						logger.LogWarning("Number of columns in the exchange rate data set from CNB is not to the expected value, so it cannot be parsed.");
						return Array.Empty<ExchangeRate>();
					}
				};

				logger.LogInformation("Exchange rate data from CNB has been successfully parsed.");
				return rates.ToArray();
			}
			else 
			{
				logger.LogWarning("Number of rows in the exchange rate data set from CNB is not equal to the expected value, so it cannot be parsed.");
			}

			return Array.Empty<ExchangeRate>();
		}		
	}
}
