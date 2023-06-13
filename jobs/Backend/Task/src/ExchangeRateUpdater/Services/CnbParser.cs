using System.Globalization;
using ExchangeRateUpdater.Contracts;
using FuncSharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
	public class CnbParser : ICnbParser
	{
		private const int HeaderRowCount = 2;
		private const string RateRowSeparator = "|";
		private const int RateRowColumnCount = 5;
		private const int SourceCurrencyAmountColumnIndex = 2;
		private const int SourceCurrencyCodeColumnIndex = 3;
		private const int ExchangeRateColumnIndex = 4;
		private readonly Currency targetCurrency = new("CZK");
		private readonly CultureInfo cultureInfo;
		private readonly ILogger<CnbParser> logger;

		public CnbParser(IOptions<CnbDailyRatesOptions> options, ILogger<CnbParser> logger)
		{
			cultureInfo = CultureInfo.CreateSpecificCulture(options.Value.CultureName!);
			this.logger = logger;
		}

		public IEnumerable<ExchangeRate> Parse(string data)
		{
			ITry<ExchangeRate, string>[] rates = GetLines(data)
				.Skip(HeaderRowCount)
				.Select(ParseRow)
				.ToArray();

			if (!rates.Any())
			{
				logger.LogWarning("No exchange rates were available");
				return Enumerable.Empty<ExchangeRate>();
			}

			var errorRates = rates
				.Where(rate => rate.IsError)
				.ToArray();
			if (errorRates.Any())
			{
				foreach (var errorRate in errorRates)
				{
					logger.LogWarning("Could not parse exchange rate: '{Error}'", errorRate.Error.Get() ?? "Unknown error");
				}
			}

			return rates
				.Where(rate => rate.IsSuccess)
				.Select(rate => rate.Success.Get());
		}

		private static IEnumerable<string> GetLines(string data)
		{
			// Using StringReader handles different line endings (CR, LF, CRLF)
			var reader = new StringReader(data);
			while (reader.ReadLine() is {} line)
			{
				yield return line;
			}
		}

		// private (ExchangeRate? rate, string? error) ParseRow(string rateRow)
		private ITry<ExchangeRate, string> ParseRow(string rateRow)
		{
			string[] columns = rateRow.Split(RateRowSeparator);
			if (columns.Length != RateRowColumnCount)
			{
				return Error($"Invalid number of columns in row '{rateRow}'");
			}

			if (!decimal.TryParse(columns[SourceCurrencyAmountColumnIndex], NumberStyles.Number, cultureInfo, out decimal sourceCurrencyAmount))
			{
				return Error($"Invalid source currency amount in row '{rateRow}'");
			}

			var sourceCurrency = new Currency(columns[SourceCurrencyCodeColumnIndex]);

			if (!decimal.TryParse(columns[ExchangeRateColumnIndex], NumberStyles.Number, cultureInfo, out decimal exchangeRate))
			{
				return Error($"Invalid exchange rate in row '{rateRow}'");
			}

			if (sourceCurrencyAmount <= 0)
			{
				return Error($"Invalid source currency amount in row '{rateRow}'");
			}

			var fxRate = new ExchangeRate(sourceCurrency, targetCurrency, exchangeRate / sourceCurrencyAmount);
			return Try.Success<ExchangeRate, string>(fxRate);

			ITry<ExchangeRate, string> Error(string message) => Try.Error<ExchangeRate, string>(message);
		}
	}
}