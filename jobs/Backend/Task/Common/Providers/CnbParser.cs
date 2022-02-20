using System.Globalization;
using System.Text.RegularExpressions;
using Common.Logs;
using Common.Model;

namespace Common.Providers;

public class CnbParser
{
	#region Fields

	private const string TargetCurrencyCode = "CZK";
	private const char Separator = '|';

	#endregion

	/// <summary>
	/// Parse list of string in CNB format.
	/// </summary>
	/// <param name="data">String CNB data.</param>
	/// <param name="currenciesList">List of currencies which should be considered as an output.</param>
	/// <returns>Enumerable of loaded and filtered exchange rates. The Enumerable does only contains data defined by <see cref="currenciesList"/> parameter.</returns>
	/// <exception cref="InvalidDataException">Throws exception of data are not in expected format.</exception>
	public IEnumerable<ExchangeRate> Parse(string data, List<Currency> currenciesList)
	{
		// First validate the data.
		if (!Validate(data))
		{
			Log.Instance.Error("Data are not in valid format");
			throw new InvalidDataException("CNB data are not in expected format.");
		}
			
		string[] allLines = data.Split('\n');
		// The reason why have to parse this date is because sometimes CNB does not yet provide data for current date. Therefore we must read such information from the response.
		DateTime rateTime = ParseDateTime(allLines.First());

		// First two lines are header + dateTime. 
		IEnumerable<string> rateLines = allLines.Skip(2).Where(p => !string.IsNullOrEmpty(p));
		foreach (string rateLine in rateLines)
		{
			ExchangeRate? rate = ParseExchangeRate(rateLine, rateTime);
			// Skip all rates, which were not in among requested currencies.
			if (rate == null || !currenciesList.Any(p => p.Code.Equals(rate.SourceCurrency.Code)))
				continue;

			yield return rate;
		}
	}

	/// <summary>
	/// Validate CNB data string.
	/// </summary>
	/// <param name="data">Input data.</param>
	/// <returns>True if data are valid. False if data are invalid.</returns>
	public bool Validate(string data)
	{
		try
		{
			// Extract date row.
			string date = data.Substring(0, data.IndexOf('\n'));
			data = data.Remove(0, date.Length + 1);

			// Validate date row.
			bool match = Regex.Match(date, @"\d{2} [a-zA-Z]{3} \d{4}.*").Success;
			if (!match)
				return false;

			// Extract header row.
			string header = data.Substring(0, data.IndexOf('\n'));
			data = data.Remove(0, header.Length + 1);

			// Validate header row.
			match = header.Equals("Country|Currency|Amount|Code|Rate");
			if (!match)
				return false;

			// Validate exchange rate data rows.
			Regex regex = new Regex(@"[a-zA-Z ]+\|[a-zA-Z ]+\|\d+\|[a-zA-Z]{3}\|[0-9.]*");
			MatchCollection matches = regex.Matches(data);

			// -1 because last line is empty string (due to \n at the end of the last line).
			int len = data.Split('\n').Length - 1;

			return len == matches.Count;
		}
		catch (Exception)
		{
			return false;
		}
	}

	/// <summary>
	/// Parse string to DateTime.
	/// </summary>
	/// <param name="stringDate">Date time in "dd MMM yyyy" format.</param>
	/// <returns></returns>
	private DateTime ParseDateTime(string stringDate)
	{
		string corrected = string.Join(' ', stringDate.Split(' ').Take(3));
		DateTime parsed = DateTime.ParseExact(corrected, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
		return parsed;
	}

	/// <summary>
	/// Parse string retrieved from CNB into ExchangeRate object.
	/// </summary>
	/// <param name="line">Parsed string.</param>
	/// <param name="dateTime">Date which should be used for ExchangeRate instantiation.</param>
	/// <returns></returns>
	private ExchangeRate? ParseExchangeRate(string line, DateTime dateTime)
	{
		// Split line into columns.
		string[] columns = line.Split(Separator);
		if (columns.Length < 5)
		{
			Log.Instance.Warning($"Field count is less then 5. Line: {line}");
			return null;
		}

		// Parse columns.
		string stringMultiplicator = columns[2];
		string currency = columns[3];
		string valueString = columns[4].Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);

		bool parsed = int.TryParse(stringMultiplicator, out int multiplicator);
		parsed &= decimal.TryParse(valueString, out decimal value);

		if (!parsed)
		{
			Log.Instance.Warning($"Could not parse the line: {line}");
			return null;
		}

		Currency sourceCurrency = new Currency(currency);
		Currency targetCurrency = new Currency(TargetCurrencyCode);
		ExchangeRate rate = new ExchangeRate(sourceCurrency, targetCurrency, value, dateTime, multiplicator);
		return rate;
	}
}