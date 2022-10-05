using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
								new Currency(sourceCurrencyCode),
								new Currency(rateRowColumns[targetCurrencyCodeIndex]),
								Convert.ToInt16(rateRowColumns[targetCurrencyUnitAmountIndex], culture.GetCultureInfo()),
								Convert.ToDecimal(rateRowColumns[exchangeRateValueIndex], culture.GetCultureInfo())));
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
